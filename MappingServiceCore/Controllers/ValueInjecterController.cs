using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MappingServiceCore.Services.Interfaces;
using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;
using MappingServiceCore.Mappings.ValueInjecterMapping;

namespace MappingServiceCore.Controllers
{
    public class ValueInjecterController : Controller
    {
        #region Ctor

        private readonly IMappingService _mappingService;
        private readonly ValueInjecterConfig _valueInjecterConfig;

        public ValueInjecterController(
            IMappingService mappingService,
            ValueInjecterConfig valueInjecterConfig)
        {
            _mappingService = mappingService;
            _valueInjecterConfig = valueInjecterConfig;
        }

        #endregion

        // GET: ValueInjecter
        [Route("value-injecter-list")]
        public async Task<IActionResult> Index(string? searchQuery)
        {
            try
            {
                var person = await _mappingService.GetAllPersonsAsync()
                          ?? Enumerable.Empty<Person>();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    person = await _mappingService.SearchPersonsAsync(searchQuery);

                    ViewData["CurrentFilter"] = searchQuery;
                }

                var peopleDto = person?.Select(personEntity => _valueInjecterConfig
                                       .MapEntityToDto(personEntity))
                                       .ToList() ??
                                       new List<PersonDto>();

                var peopleList = peopleDto?.Select(personDto => _valueInjecterConfig
                                           .MapDtoToViewModel(personDto))
                                           .ToList() ??
                                           new List<PersonViewModel>();

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

        // GET: ValueInjecter/Create
        [Route("value-injecter-add")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ValueInjecter/Create
        [HttpPost]
        [Route("value-injecter-add")]
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

                var personDto = _valueInjecterConfig.MapViewModelToDto(model);

                personDto.CreateDate = DateTime.Now.ToString();

                var personEntity = _valueInjecterConfig.MapDtoToEntity(personDto);

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

        // GET: ValueInjecter/Details/5
        [Route("value-injecter-detail")]
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

                var personDto = _valueInjecterConfig.MapEntityToDto(personEntity);

                var personVm = _valueInjecterConfig.MapDtoToViewModel(personDto);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام واکشی اطلاعات شخص خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ValueInjecter/Edit/5
        [Route("value-injecter-update")]
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

                var personDto = _valueInjecterConfig.MapEntityToDto(personEntity);

                var personVm = _valueInjecterConfig.MapDtoToViewModel(personDto);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام آماده‌سازی شخص برای به‌روزرسانی، خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ValueInjecter/Edit/5
        [HttpPost]
        [Route("value-injecter-update")]
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
                var personDto = _valueInjecterConfig.MapViewModelToDto(model);

                var personEntity = _valueInjecterConfig.MapDtoToEntity(personDto);

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

        // GET: ValueInjecter/Delete/5
        [Route("value-injecter-remove")]
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

                var personDto = _valueInjecterConfig.MapEntityToDto(personEntity);

                var personVm = _valueInjecterConfig.MapDtoToViewModel(personDto);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام بازیابی شخص برای حذف، خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ValueInjecter/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("value-injecter-remove")]
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