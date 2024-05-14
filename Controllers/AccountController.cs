using LutongBahayApp.Data;
using LutongBahayApp.Interfaces;
using LutongBahayApp.Models;
using LutongBahayApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LutongBahayApp.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAccountRepository accountRepository, IHttpContextAccessor contextAccessor) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Input";
                return View(loginViewModel);
            }

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                var password = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (password)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                    if (result.Succeeded)
                    {
                        var returnUrl = _contextAccessor.HttpContext.Request.Query["returnUrl"].ToString();

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            // Redirect the user back to the returnUrl (the basket page or another page)
                            return Redirect(returnUrl);
                        }

                        // If returnUrl is null or empty, redirect the user to a default page
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["Error"] = "Wrong credentials";
                return View(loginViewModel);
            }

            TempData["Error"] = "Wrong credentials";
            return View(loginViewModel);
        }

        public IActionResult RegisterCustomer()
        {
            var response = new RegisterCustomerViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerViewModel registerCustomerViewModel)
        {
            if (!ModelState.IsValid) return View(registerCustomerViewModel);

            var user = await _userManager.FindByEmailAsync(registerCustomerViewModel.Email);
            if (user != null)
            {
                TempData["Errors"] = "This email address is already in use";
                return View(registerCustomerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerCustomerViewModel.Email,
                UserName = registerCustomerViewModel.Username,
                FirstName = registerCustomerViewModel.FirstName,
                LastName = registerCustomerViewModel.LastName,
                Address = registerCustomerViewModel.Address,
                PhoneNumber = registerCustomerViewModel.PhoneNumber,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerCustomerViewModel.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Customer);
            else
            {
                foreach (var error in newUserResponse.Errors)
                {
                    var errorMessages = newUserResponse.Errors.Select(error => error.Description).ToArray();
                    TempData["Errors"] = errorMessages;
                }
                return View(registerCustomerViewModel);
            }

            return RedirectToAction("Login", "Account");
        }

        public IActionResult RegisterSeller()
        {
            var response = new RegisterSellerViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSeller(RegisterSellerViewModel registerSellerViewModel)
        {
            if (!ModelState.IsValid) return View(registerSellerViewModel);

            var user = await _userManager.FindByEmailAsync(registerSellerViewModel.Email);
            if (user != null)
            {
                TempData["Errors"] = "This email address is already in use";
                return View(registerSellerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerSellerViewModel.Email,
                UserName = registerSellerViewModel.Username,
                FirstName = registerSellerViewModel.FirstName,
                LastName = registerSellerViewModel.LastName,
                Address = registerSellerViewModel.Address,
                PhoneNumber = registerSellerViewModel.PhoneNumber,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerSellerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Seller);

                var userMarket = new Market()
                {
                    Name = registerSellerViewModel.MarketName,
                    Address = registerSellerViewModel.MarketAddress,
                    Location = registerSellerViewModel.MarketLocation,
                    UserId = newUser.Id
                };

                _accountRepository.AddMarketUser(userMarket);
            }
            else
            {
                foreach (var error in newUserResponse.Errors)
                {
                    var errorMessages = newUserResponse.Errors.Select(error => error.Description).ToArray();
                    TempData["Errors"] = errorMessages;
                }
                return View(registerSellerViewModel);
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
