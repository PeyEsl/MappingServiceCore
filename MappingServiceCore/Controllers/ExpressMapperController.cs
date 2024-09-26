using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;
using ExpressMapper;

namespace MappingServiceCore.Controllers
{
    public class ExpressMapperController : Controller
    {
        #region Ctor

        private readonly MappingServiceCore.Services.Interfaces.IMappingService _mappingService;

        public ExpressMapperController(
            MappingServiceCore.Services.Interfaces.IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        #endregion

        // GET: ExpressMapper
        [Route("express-mapper-list")]
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

                var peopleDto = person?.Select(personEntity => Mapper
                                       .Map<Person, PersonDto>(personEntity))
                                       .ToList() ??
                                       new List<PersonDto>();

                var peopleList = peopleDto?.Select(personDto => Mapper
                                           .Map<PersonDto, PersonViewModel>(personDto))
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

        // GET: ExpressMapper/Create
        [Route("express-mapper-add")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExpressMapper/Create
        [HttpPost]
        [Route("express-mapper-add")]
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

                var personDto = Mapper.Map<PersonViewModel, PersonDto>(model);

                personDto.CreateDate = DateTime.Now.ToString();

                var personEntity = Mapper.Map<PersonDto, Person>(personDto);

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

        // GET: ExpressMapper/Details/5
        [Route("express-mapper-detail")]
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

                var personDto = Mapper.Map<Person, PersonDto>(personEntity);

                var personVm = Mapper.Map<PersonDto, PersonViewModel>(personDto);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام واکشی اطلاعات شخص خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ExpressMapper/Edit/5
        [Route("express-mapper-update")]
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

                var personDto = Mapper.Map<Person, PersonDto>(personEntity);

                var personVm = Mapper.Map<PersonDto, PersonViewModel>(personDto);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام آماده‌سازی شخص برای به‌روزرسانی، خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ExpressMapper/Edit/5
        [HttpPost]
        [Route("express-mapper-update")]
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
                var personDto = Mapper.Map<PersonViewModel, PersonDto>(model);

                var personEntity = Mapper.Map<PersonDto, Person>(personDto);

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

        // GET: ExpressMapper/Delete/5
        [Route("express-mapper-remove")]
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

                var personDto = Mapper.Map<Person, PersonDto>(personEntity);

                var personVm = Mapper.Map<PersonDto, PersonViewModel>(personDto);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام بازیابی شخص برای حذف، خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ExpressMapper/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("express-mapper-remove")]
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