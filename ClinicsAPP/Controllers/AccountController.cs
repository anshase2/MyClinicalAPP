using ClinicsAPP.DTO;
using ClinicsAPP.Models;
using ClinicsAPP.Models.IdentityModels;
using ClinicsAPP.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ClinicsAPP.Contracts;



namespace ClinicsAPP.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly DoctorService _doctorService;
        private readonly IPatientService _PatientService;
        private readonly ImageService _imageService;





        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, DoctorService doctorService, IPatientService patientService, ImageService imageService)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _imageService = imageService;
            _doctorService = doctorService;
            _PatientService = patientService;

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (registerDTO.Role == Contracts.UserTypeOptions.Doctor)
            {
                if (registerDTO.Doctor == null)
                {
                    ModelState.AddModelError("", "Doctor data is required.");
                }
                else
                {
                    if (!registerDTO.Doctor.Price.HasValue)
                        ModelState.AddModelError("Doctor.Price", "Price is required");

                    if (string.IsNullOrEmpty(registerDTO.Doctor.Specalist))
                        ModelState.AddModelError("Doctor.Specalist", "Specialty is required");

                    if (string.IsNullOrEmpty(registerDTO.Doctor.Location))
                        ModelState.AddModelError("Doctor.Location", "Location is required");
                    if (registerDTO.Doctor.ImageUrl == null || registerDTO.Doctor.ImageUrl.Length == 0)
                        ModelState.AddModelError("Doctor.ImageUrl", "Profile image is required");
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(registerDTO);
            }

            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(registerDTO.ConfirmPassword), "Password and confirmation do not match.");
                return View(registerDTO);
            }
            var phoneExists = await _userManager.Users
       .AnyAsync(u => u.PhoneNumber == registerDTO.Phone);

            if (phoneExists)
            {
                ModelState.AddModelError(nameof(registerDTO.Phone), "Phone number is already in use.");
                return View(registerDTO);
            }

            var emailExists = await _userManager.FindByEmailAsync(registerDTO.Email);

            if (emailExists != null)
            {
                ModelState.AddModelError(nameof(registerDTO.Email), "Email is already in use.");
                return View(registerDTO);
            }

            var user = new ApplicationUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Gender = registerDTO.Gender,
                DateOfBirth = registerDTO.DateOfBirth
            };

           
            

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            
            if (result.Succeeded)
            {
                string role = registerDTO.Role.ToString();
                

                // تأكد أن role موجود
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new ApplicationRole() {Name=role });
                }

                // إضافة المستخدم للـ role
                await _userManager.AddToRoleAsync(user, role);
                if (role == "Doctor")
                {
                    if(registerDTO.Doctor.ImageUrl == null)
                    {
                        ModelState.AddModelError("Doctor.ImageUrl", "Profile image is required");
                        return View(registerDTO);
                    }
                    var Doctor = new DoctorRequestDTO
                    {

                        FullName = registerDTO.FirstName + " " + registerDTO.LastName,
                        Location = registerDTO.Doctor.Location,
                        Price = registerDTO.Doctor.Price,
                        ImageUrl=await _imageService.UploadAsync(registerDTO.Doctor.ImageUrl),
                        // ImageUrl = await _imageService.UploadAsync(registerDTO.Doctor.ImageUrl) ,
                        Specalist = registerDTO.Doctor.Specalist,
                        UserId = user.Id,
                        Description = registerDTO.Doctor.Description??""
                    };
                    await _doctorService.CreateDoctorAsync(Doctor);

                }
                else if (role == "Patient") {
                    var patient = new PatientRequestDTO { FullName = registerDTO.FirstName + " " + registerDTO.LastName ,UserId =user.Id};

                    await _PatientService.CreatePatient(patient);



                }
                await _signInManager.SignInAsync(user, isPersistent: false);

                await _userManager.AddClaimAsync(
     user,
     new Claim("FullName", $"{user.FirstName} {user.LastName}")
 );

                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction(nameof(HomeController.Home), "Home");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

           

            return View(registerDTO);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO,string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }


           
           
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl)) {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction(nameof(HomeController.Home), "Home");
            }

            ModelState.AddModelError("Login", "Inavlid email or password");
            return View(loginDTO);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
