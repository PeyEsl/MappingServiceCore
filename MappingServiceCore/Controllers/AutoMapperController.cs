using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MappingServiceCore.Services.Interfaces;
using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;
using AutoMapper;

namespace MappingServiceCore.Controllers
{
    public class AutoMapperController : Controller
    {
        #region Ctor

        private readonly IMappingService _mappingService;
        private readonly IMapper _mapper;

        public AutoMapperController(
            IMappingService mappingService,
            IMapper mapper)
        {
            _mappingService = mappingService;
            _mapper = mapper;
        }

        #endregion

        // GET: AutoMapper
        [Route("auto-mapper-list")]
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

                var peopleDto = person?.Select(personEntity => _mapper
                                       .Map<PersonDto>(personEntity))
                                       .ToList() ??
                                       new List<PersonDto>();

                var peopleList = peopleDto?.Select(personDto => _mapper
                                           .Map<PersonViewModel>(personDto))
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

        // GET: AutoMapper/Create
        [Route("auto-mapper-add")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AutoMapper/Create
        [HttpPost]
        [Route("auto-mapper-add")]
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

                var personDto = _mapper.Map<PersonDto>(model);

                personDto.CreateDate = DateTime.Now.ToString();

                var personEntity = _mapper.Map<Person>(personDto);

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

        // GET: AutoMapper/Details/5
        [Route("auto-mapper-detail")]
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

                var personDto = _mapper.Map<PersonDto>(personEntity);

                var personVm = _mapper.Map<PersonViewModel>(personDto);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام واکشی اطلاعات شخص خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AutoMapper/Edit/5
        [Route("auto-mapper-update")]
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

                var personDto = _mapper.Map<PersonDto>(personEntity);

                var personVm = _mapper.Map<PersonViewModel>(personDto);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام آماده‌سازی شخص برای ویرایش، خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AutoMapper/Edit/5
        [HttpPost]
        [Route("auto-mapper-update")]
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
                var personDto = _mapper.Map<PersonDto>(model);

                var personEntity = _mapper.Map<Person>(personDto);

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

        // GET: AutoMapper/Delete/5
        [Route("auto-mapper-remove")]
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

                var personDto = _mapper.Map<PersonDto>(personEntity);

                var personVm = _mapper.Map<PersonViewModel>(personDto);

                return View(personVm);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "هنگام بازیابی شخص برای حذف، خطایی رخ داد. لطفاً بعداً دوباره امتحان کنید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AutoMapper/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("auto-mapper-remove")]
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
