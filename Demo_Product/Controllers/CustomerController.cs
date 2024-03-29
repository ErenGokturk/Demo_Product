﻿using BusinessLayer.Concrete;
using BusinessLayer.FluentValidation;
using DataAccessLayer.EntityFrameWork;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.WebRequestMethods;

namespace Demo_Product.Controllers
{
    public class CustomerController : Controller
    {
        CustomerManager customerManager = new CustomerManager(new EfCustomerDal());
        JobManager jobManager = new JobManager( new EfJobDal());
        public IActionResult Index()
        {
            var values = customerManager.GetCustomersListWithJob();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddCustomer()
        { 
            
            List<SelectListItem> value = (from x in jobManager.TGetList()
                                          select new SelectListItem
                                          {
                                              Text=x.Name,
                                              Value=x.JobID.ToString()
                                          }).ToList();
            ViewBag.v = value;                            
            return View();
        }
        [HttpPost]
        public IActionResult AddCustomer(Customer p ) 
        {

            CustomerValidator validationRules = new CustomerValidator();
            ValidationResult result = validationRules.Validate(p);
            if (result.IsValid)
            {
                     customerManager.TInsert(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }    
            }
            return View();
        }


  
        public IActionResult DeleteCustomer(int id) 
        {  
            var value=customerManager.TGetById(id);
            customerManager.TDelete(value);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UpdateCustomer(int id) 
        {
            List<SelectListItem> value = (from x in jobManager.TGetList()
                                           select new SelectListItem
                                           {
                                               Text = x.Name,
                                               Value = x.JobID.ToString()
                                           }).ToList();
            ViewBag.v = value;
            var values = customerManager.TGetById(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateCustomer(Customer p)
        {
            customerManager.TUpdate(p);
            return RedirectToAction("Index");
        }
    }
}
