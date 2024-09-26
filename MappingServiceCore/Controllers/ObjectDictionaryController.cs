using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MappingServiceCore.Services.Interfaces;
using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.ViewModels;
using MappingServiceCore.Mappings.ObjectToDictionaryMapping;

namespace MappingServiceCore.Controllers
{
    public class ObjectDictionaryController : Controller
    {
        #region Ctor

        private readonly IMappingService _mappingService;
        private readonly ObjectDictionaryReflection _objectDictionaryReflection;
        private readonly ObjectDictionaryJson _objectDictionaryJson;
        private readonly ObjectToDictionaryLinq _objectToDictionaryLinq;
        private readonly ObjectToDictionaryManual _objectToDictionaryManual;

        public ObjectDictionaryController(
            IMappingService mappingService,
            ObjectDictionaryReflection objectDictionaryReflection,
            ObjectDictionaryJson objectDictionaryJson,
            ObjectToDictionaryLinq objectToDictionaryLinq,
            ObjectToDictionaryManual objectToDictionaryManual)
        {
            _mappingService = mappingService;
            _objectDictionaryReflection = objectDictionaryReflection;
            _objectDictionaryJson = objectDictionaryJson;
            _objectToDictionaryLinq = objectToDictionaryLinq;
            _objectToDictionaryManual = objectToDictionaryManual;
        }

        #endregion

        // GET: ObjectDictionary
        [Route("object-to-dictionary-list")]
        public async Task<IActionResult> Index(string? searchQuery)
        {
            try
            {
                var personEntity = await _mappingService.GetAllPersonsAsync()
                                ?? Enumerable.Empty<Person>();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    personEntity = await _mappingService.SearchPersonsAsync(searchQuery);

                    ViewData["CurrentFilter"] = searchQuery;
                }

                var peopleDictionary = personEntity.Select(person => _objectDictionaryReflection
                                                   .ObjectToDictionary(person)).ToList();

                var peopleList = peopleDictionary.Select(person => _objectDictionaryJson
                                                 .DictionaryToObject<PersonViewModel>(person)).ToList();

                if (TempData["SuccessMessage"] != null)
                    ViewData["SuccessMessage"] = TempData["SuccessMessage"];

                if (TempData["ErrorMessage"] != null)
                    ViewData["ErrorMessage"] = TempData["ErrorMessage"];

                return View(peopleList);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام واکشی اشخاص خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction("Mapping", "Home");
            }
        }

        // GET: ObjectDictionary/Create
        [Route("object-to-dictionary-add")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ObjectDictionary/Create
        [HttpPost]
        [Route("object-to-dictionary-add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "اطلاعات وارد شده معتبر نمی‌باشد.");

                return View(model);
            }

            try
            {
                if (await _mappingService.ExistsPersonByNameAsync(model.FirstName!))
                {
                    ModelState.AddModelError(string.Empty, "شخص با این نام از قبل وجود دارد.");

                    return View(model);
                }

                if (await _mappingService.ExistsPersonByNameAsync(model.LastName!))
                {
                    ModelState.AddModelError(string.Empty, "شخص با این نام‌خانوادگی از قبل وجود دارد.");

                    return View(model);
                }

                if (await _mappingService.ExistsPersonByNameAsync(model.PhoneNumber!))
                {
                    ModelState.AddModelError(string.Empty, "شخص با این شماره تلفن از قبل وجود دارد.");

                    return View(model);
                }

                var personDictionary = _objectToDictionaryLinq.ObjectToDictionary(model);

                var personEntity = _objectToDictionaryManual.DictionaryToPerson(personDictionary);

                personEntity.CreateDate = DateTime.Now;

                var result = await _mappingService.AddPersonAsync(personEntity);
                if (result)
                    TempData["SuccessMessage"] = "شخص با موفقیت ثبت شد.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام ایجاد شخص خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ObjectDictionary/Details/5
        [Route("object-to-dictionary-detail")]
        public async Task<IActionResult> Details(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "شناسه شخص پوچ است.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var personEntity = await _mappingService.GetPersonByIdAsync(id);
                if (personEntity == null)
                {
                    TempData["ErrorMessage"] = $"شخص با شناسه {id} یافت نشد.";

                    return RedirectToAction(nameof(Index));
                }

                var personDictionary = _objectToDictionaryManual.PersonToDictionary(personEntity);

                var personVm = _objectToDictionaryLinq.DictionaryToObject<PersonViewModel>(personDictionary);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام واکشی اطلاعات شخص خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ObjectDictionary/Edit/5
        [Route("object-to-dictionary-update")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "شناسه شخص پوچ است.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var personEntity = await _mappingService.GetPersonByIdAsync(id);
                if (personEntity == null)
                {
                    TempData["ErrorMessage"] = $"شخص با شناسه {id} یافت نشد.";

                    return RedirectToAction(nameof(Index));
                }

                var personDictionary = _objectToDictionaryManual.PersonToDictionary(personEntity);

                var personVm = _objectDictionaryJson.DictionaryToObject<PersonViewModel>(personDictionary);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام آماده‌سازی شخص برای ویرایش، خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ObjectDictionary/Edit/5
        [HttpPost]
        [Route("object-to-dictionary-update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, PersonViewModel model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "شناسه‌ها برابر نیستند.";

                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "اطلاعات وارد شده معتبر نمی‌باشد.");

                return View(model);
            }

            try
            {
                var personDictionary = _objectDictionaryReflection.ObjectToDictionary(model);

                var personEntity = _objectToDictionaryManual.DictionaryToPerson(personDictionary);

                var result = await _mappingService.UpdatePersonAsync(personEntity);
                if (result)
                    TempData["SuccessMessage"] = "شخص با موفقیت به‌روزرسانی شد.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PersonExistsAsync(model.Id))
                {
                    TempData["ErrorMessage"] = "شخص در هنگام عملیات به‌روزرسانی پیدا نشد.";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "هنگام به‌روزرسانی شخص، خطای همزمانی رخ داد. لطفاً بعداً دوباره امتحان کنید.";

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام به‌روزرسانی شخص خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ObjectDictionary/Delete/5
        [Route("object-to-dictionary-remove")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "شناسه شخص پوچ است.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var personEntity = await _mappingService.GetPersonByIdAsync(id);
                if (personEntity == null)
                {
                    TempData["ErrorMessage"] = $"شخص با شناسه {id} یافت نشد.";

                    return RedirectToAction(nameof(Index));
                }

                var personDictionary = _objectToDictionaryLinq.ObjectToDictionary(personEntity);

                var personVm = _objectDictionaryJson.DictionaryToObject<PersonViewModel>(personDictionary);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام بازیابی شخص برای حذف، خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ObjectDictionary/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("object-to-dictionary-remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "شناسه شخص پوچ است.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var personEntity = await _mappingService.GetPersonByIdAsync(id);
                if (personEntity == null)
                {
                    TempData["ErrorMessage"] = $"شخص با شناسه {id} یافت نشد.";

                    return RedirectToAction(nameof(Index));
                }

                var result = await _mappingService.DeletePersonAsync(id);
                if (result)
                    TempData["SuccessMessage"] = "شخص با موفقیت حذف شد.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام بازیابی شخص برای حذف، خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<bool> PersonExistsAsync(string id)
        {
            return await _mappingService.ExistsPersonAsync(id);
        }
    }
}