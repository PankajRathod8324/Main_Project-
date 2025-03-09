using System.Diagnostics;
using System.Security.Claims;
using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
// using PizzaShop.Models;
using X.PagedList.Extensions;

namespace PizzaShop.Controllers;


public class MenuController : Controller
{
    private readonly IMenuService _menuService;


    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [Authorize(Policy = "MenuEditPolicy")]
    public IActionResult MenuItem(int categoryId, string search = "", int page = 1, string sortOrder = "")
    {

        var categories = _menuService.GetAllCategories(); // Fetch Categories from the database
        var categorySelectList = categories.Select(r => new SelectListItem
        {
            Value = r.CategoryId.ToString(),
            Text = r.CategoryName
        }).ToList();
        ViewBag.Categories = categorySelectList;

        var itemtypes = _menuService.GetAllItemTypes(); // Fetch ItemTypes from the database
        var itemtypeSelectList = itemtypes.Select(r => new SelectListItem
        {
            Value = r.ItemtypeId.ToString(),
            Text = r.ItemType1
        }).ToList();
        ViewBag.Itemtypes = itemtypeSelectList;

        var units = _menuService.GetAllUnits(); // Fetch Units from the database
        var unitSelectList = units.Select(r => new SelectListItem
        {
            Value = r.UnitId.ToString(),
            Text = r.UnitName
        }).ToList();
        ViewBag.Units = unitSelectList;

        var modifiergroups = _menuService.GetAllModifierGroups(); // Fetch Units from the database
        var modifiergroupSelectList = modifiergroups.Select(r => new SelectListItem
        {
            Value = r.ModifierGroupId.ToString(),
            Text = r.ModifierGroupName
        }).ToList();
        ViewBag.ModifierGroups = modifiergroupSelectList;


        Console.WriteLine("Click this : " + categoryId);
        // categoryId = categoryId == 0 ? 11 : categoryId;
        Console.WriteLine("I am in Controoerkekl : " + search);
        var unitName = _menuService.GetAllUnits().ToList();
        var menuItems = _menuService.GetItemsByCategoryId(categoryId).AsQueryable();
        int pageSize = 5;

        // Search filter
        if (!string.IsNullOrEmpty(search))
        {
            search = search.Trim().ToLower();
            menuItems = menuItems.Where(i => i.ItemName.ToLower().Contains(search));
        }

        Console.WriteLine(menuItems);

        // Sorting
        ViewBag.NameSort = sortOrder == "name_asc" ? "name_desc" : "name_asc";
        ViewBag.RateSort = sortOrder == "rate_asc" ? "rate_desc" : "rate_asc";

        switch (sortOrder)
        {
            case "name_asc":
                menuItems = menuItems.OrderBy(i => i.ItemName);
                break;
            case "name_desc":
                menuItems = menuItems.OrderByDescending(i => i.ItemName);
                break;
            case "rate_asc":
                menuItems = menuItems.OrderBy(i => i.Rate);
                break;
            case "rate_desc":
                menuItems = menuItems.OrderByDescending(i => i.Rate);
                break;
            default:
                menuItems = menuItems.OrderBy(i => i.ItemId);
                break;
        }




        // Project to ViewModel before pagination
        var paginateditems = menuItems
            .Select(item => new MenuItem
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                UnitId = item.UnitId,
                CategoryId = item.CategoryId,
                ItemtypeId = item.ItemtypeId,
                Rate = item.Rate,
                Quantity = item.Quantity,
                IsAvailable = (bool)item.IsAvailable,
                TaxDefault = item.TaxDefault,
                TaxPercentage = item.TaxPercentage,
                ShortCode = item.ShortCode,
                Description = item.Description
                // UnitName =  item.UnitId.HasValue ? _menuService.GetUnitById(item.UnitId.Value) : "No Unit"

            }).ToPagedList(page, pageSize);


        foreach (var item in paginateditems)
        {
            Console.WriteLine(item.ItemName);
        }

        MenuCategoryVM itemvm = new MenuCategoryVM
        {
            menuItems = paginateditems
        };

        return PartialView("_MenuItemPV", itemvm);
    }

    
    [Authorize(Policy = "MenuEditPolicy")]
    public IActionResult EditMenuItem(int itemId)
    {

        var categories = _menuService.GetAllCategories(); // Fetch Categories from the database
        var categorySelectList = categories.Select(r => new SelectListItem
        {
            Value = r.CategoryId.ToString(),
            Text = r.CategoryName
        }).ToList();
        ViewBag.Categories = categorySelectList;

        var itemtypes = _menuService.GetAllItemTypes(); // Fetch ItemTypes from the database
        var itemtypeSelectList = itemtypes.Select(r => new SelectListItem
        {
            Value = r.ItemtypeId.ToString(),
            Text = r.ItemType1
        }).ToList();
        ViewBag.Itemtypes = itemtypeSelectList;

        var units = _menuService.GetAllUnits(); // Fetch Units from the database
        var unitSelectList = units.Select(r => new SelectListItem
        {
            Value = r.UnitId.ToString(),
            Text = r.UnitName
        }).ToList();
        ViewBag.Units = unitSelectList;

        var modifiergroups = _menuService.GetAllModifierGroups(); // Fetch Units from the database
        var modifiergroupSelectList = modifiergroups.Select(r => new SelectListItem
        {
            Value = r.ModifierGroupId.ToString(),
            Text = r.ModifierGroupName
        }).ToList();
        ViewBag.ModifierGroups = modifiergroupSelectList;


        Console.WriteLine("Click this in edit  : " + itemId);
        var item = _menuService.GetItemById(itemId);



        var itemmodifier = _menuService.GetItemModifier(item.ItemId, (int)item.ModifierGroupId);

        var itemvm = new MenuCategoryVM
        {
            CategoryId = item.CategoryId,
            ItemId = item.ItemId,
            ItemName = item.ItemName,
            ItemtypeId = item.ItemtypeId,
            Rate = item.Rate,
            Quantity = item.Quantity,
            UnitId = item.UnitId,
            IsAvailable = (bool)item.IsAvailable,
            TaxDefault = item.TaxDefault,
            TaxPercentage = item.TaxPercentage,
            ShortCode = item.ShortCode,
            Description = item.Description,
            ModifierGroupId = item.ModifierGroupId,
            MinSelection = (int)itemmodifier.MinSelection,
            MaxSelection = (int)itemmodifier.MaxSelection
        };

        return PartialView("_EditItemPV", itemvm);
    }


    
    [Authorize(Policy = "MenuEditPolicy")]
    public IActionResult EditItem(MenuCategoryVM model)
    {
        Console.WriteLine("Edit name:" + model.ItemId);
        Console.WriteLine("Edit name:" + model.ItemName);
        // Console.WriteLine("Edit name:" + model.CategoryDescription);

        var item = _menuService.GetItemById(model.ItemId);
        if (item == null)
        {
            return NotFound();
        }
        Console.WriteLine("In Edit Page");

        item.CategoryId = model.CategoryId;
        item.ItemId = model.ItemId;
        item.ItemName = model.ItemName;
        item.ItemtypeId = model.ItemtypeId;
        item.Rate = model.Rate;
        item.Quantity = model.Quantity;
        item.UnitId = model.UnitId;
        item.IsAvailable = model.IsAvailable;
        item.TaxDefault = model.TaxDefault;
        item.TaxPercentage = model.TaxPercentage;
        item.ShortCode = model.ShortCode;
        item.Description = model.Description;

        _menuService.UpdateMenuItem(item);
        return RedirectToAction("Menu", "Home");
    }


    
    [Authorize(Policy = "MenuViewPolicy")]

    public IActionResult MenuCategory()
    {
        // var categories = _menuService.GetAllCategories();
        var categories = _menuService.GetAllCategories(); // Fetch Categories from the database
        var categorySelectList = categories.Select(r => new SelectListItem
        {
            Value = r.CategoryId.ToString(),
            Text = r.CategoryName
        }).ToList();
        ViewBag.Categories = categorySelectList;

        var itemtypes = _menuService.GetAllItemTypes(); // Fetch ItemTypes from the database
        var itemtypeSelectList = itemtypes.Select(r => new SelectListItem
        {
            Value = r.ItemtypeId.ToString(),
            Text = r.ItemType1
        }).ToList();
        ViewBag.Itemtypes = itemtypeSelectList;

        var units = _menuService.GetAllUnits(); // Fetch Units from the database
        var unitSelectList = units.Select(r => new SelectListItem
        {
            Value = r.UnitId.ToString(),
            Text = r.UnitName
        }).ToList();
        ViewBag.Units = unitSelectList;

        var modifiergroups = _menuService.GetAllModifierGroups(); // Fetch Units from the database
        var modifiergroupSelectList = modifiergroups.Select(r => new SelectListItem
        {
            Value = r.ModifierGroupId.ToString(),
            Text = r.ModifierGroupName
        }).ToList();
        ViewBag.ModifierGroups = modifiergroupSelectList;


        // var categories = _menuService.GetAllCategories();

        var categoryvm = new MenuCategoryVM
        {
            menuCategories = categories
        };


        // foreach (var category in categoryvm.menuCategories)
        // {
        //     Console.WriteLine(category.CategoryName);
        // }
        // return View(categories);

        return PartialView("_MenuCategoryPV", categoryvm);
    }


    // [HttpPost]
    // public IActionResult AddMenuCategory(MenuCategory model)
    // {
    //     Console.WriteLine(ModelState.IsValid);
    //     Console.WriteLine("--------------Add MenuCategory");
    //     Console.WriteLine(model.CategoryName);
    //     if (!ModelState.IsValid)
    //     {
    //         // Log the model state errors
    //         foreach (var state in ModelState)
    //         {
    //             foreach (var error in state.Value.Errors)
    //             {
    //                 Console.WriteLine($"Property: {state.Key}, Error: {error.ErrorMessage}");
    //             }
    //         }
    //         return RedirectToAction("Menu", "Home");
    //     }

    //     var category = new MenuCategory
    //     {
    //         CategoryName = model.CategoryName,
    //         CategoryDescription = model.CategoryDescription
    //     };



    //     _menuService.AddCategory(category);
    //     Console.WriteLine("--------------Add User END");
    //     TempData["Message"] = "Category added successfully!";
    //     TempData["MessageType"] = "success";

    //     return RedirectToAction("Menu", "Home");
    // }

    
    [Authorize(Policy = "MenuViewPolicy")]
    public IActionResult MenuModifier(int modifierId)
    {
        var units = _menuService.GetAllUnits(); // Fetch Units from the database
        var unitSelectList = units.Select(r => new SelectListItem
        {
            Value = r.UnitId.ToString(),
            Text = r.UnitName
        }).ToList();
        ViewBag.Units = unitSelectList;

        var modifiergroups = _menuService.GetAllModifierGroups(); // Fetch Units from the database
        var modifiergroupSelectList = modifiergroups.Select(r => new SelectListItem
        {
            Value = r.ModifierGroupId.ToString(),
            Text = r.ModifierGroupName
        }).ToList();
        ViewBag.ModifierGroups = modifiergroupSelectList;


        Console.WriteLine("Click this : " + modifierId);
        var modifiers = _menuService.GetModifiersByModifierGroupId(modifierId);
        foreach (var modifier in modifiers)
        {
            Console.WriteLine(modifier.ModifierName);
        }

        // var unitname = 

        //mapping with menuitem and modifier groups

        var modifieritems = modifiers
          .Select(item => new MenuModifierGroupVM
          {
              ModifierName = item.ModifierName,
              ModifierRate = item.ModifierRate,
              CategoryId = item.CategoryId,
              UnitId = item.UnitId,
              Quantity = item.Quantity,
              ModifierDecription = item.ModifierDecription,
              UnitName = item.UnitId.HasValue ? _menuService.GetUnitById(item.UnitId.Value) : "No Unit"

          }).ToList();



        MenuModifierGroupVM modifiervm = new MenuModifierGroupVM
        {
            menuModifiers = modifieritems
            // UnitName = modifiers.UnitId.HasValue ? _menuService.GetUnitById(modifiers.UnitId.Value) : "No Units"
        };
        return PartialView("_MenuModifierPV", modifiervm);
    }

    [Authorize]
    public IActionResult GetAllModifier()
    {
        var modifiers = _menuService.GetModifiers();
        Console.WriteLine("Modifiers");
        foreach (var modifier in modifiers)
        {
            Console.WriteLine(modifier.ModifierName);
        }

        var modifieritems = modifiers
          .Select(item => new MenuModifierGroupVM
          {
              ModifierGroupId = (int)item.ModifierGroupId,
              ModifierId = item.ModifierId,
              ModifierName = item.ModifierName,
              ModifierRate = item.ModifierRate,
              CategoryId = item.CategoryId,
              UnitId = item.UnitId,
              Quantity = item.Quantity,
              ModifierDecription = item.ModifierDecription,
              UnitName = item.UnitId.HasValue ? _menuService.GetUnitById(item.UnitId.Value) : "No Unit"

          }).ToList();

        MenuModifierGroupVM modifiervm = new MenuModifierGroupVM
        {
            menuModifiers = modifieritems
            // UnitName = modifiers.UnitId.HasValue ? _menuService.GetUnitById(modifiers.UnitId.Value) : "No Units"
        };
        return PartialView("_MenuModifierByModifierGroup", modifiervm);
    }

    [Authorize]
    public IActionResult GetModifiersByGroup(int groupId)
    {
        var modifiers = _menuService.GetModifiersByModifierGroupId(groupId);
        var groupName = _menuService.GetModifierGroupById(groupId);


        var modifieritems = modifiers
           .Select(item => new MenuModifierGroupVM
           {
               ModifierGroupId = (int)item.ModifierGroupId,

               ModifierName = item.ModifierName,
               ModifierRate = item.ModifierRate,
               CategoryId = item.CategoryId,
               UnitId = item.UnitId,
               Quantity = item.Quantity,
               ModifierDecription = item.ModifierDecription,
               UnitName = item.UnitId.HasValue ? _menuService.GetUnitById(item.UnitId.Value) : "No Unit"

           }).ToList();
        MenuModifierGroupVM modifiervm = new MenuModifierGroupVM
        {
            menuModifiers = modifieritems,
            ModifierGroupName = groupName.ModifierGroupName
            // UnitName = modifiers.UnitId.HasValue ? _menuService.GetUnitById(modifiers.UnitId.Value) : "No Units"
        };

        // var groupName = _menuService.GetModifierNameById(groupId, modifiervm);








        return PartialView("_ModifierList", modifiervm);
    }
    // public IActionResult GetModifiers(int page = 1, string search = "")
    // {
    //     int pageSize = 5;
    //     int totalPages;

    //     var modifiers = _menuService.GetModifiers(page, search, pageSize, out totalPages);

    //     return Json(new { data = modifiers, totalPages });
    // }

    
    [Authorize(Policy = "MenuViewPolicy")]
    public IActionResult MenuModifierGroup()
    {

        var modifierGroups = _menuService.GetAllModifierGroups();

        var modifierGroupvm = new MenuModifierGroupVM
        {
            menuModifierGroups = modifierGroups
        };

        return PartialView("_MenuModifierGroupPV", modifierGroupvm);
    }

    
    [Authorize(Policy = "MenuEditPolicy")]
    [HttpPost]
    public IActionResult AddMenuCategory(MenuCategory category)
    {


        // Console.WriteLine("Hekrkjnhfrkjnfcekjnb" + categoryvm.editCategories.CategoryName);
        if (ModelState.IsValid)
        {
            _menuService.AddCategory(category);
            return RedirectToAction("Menu", "Home");
        }
        return RedirectToAction("Menu", "Home");
    }

    // [HttpPost]
    // public IActionResult EditCategory(int id)
    // {
    //     Console.WriteLine(id);
    //     // Console.WriteLine(category.CategoryName);

    //     // if (ModelState.IsValid)
    //     // {
    //     //     _menuService.UpdateCategory(category);
    //     //     return RedirectToAction("Menu", "Home");
    //     // }
    //     return RedirectToAction("Menu", "Home");
    // }

    
    [Authorize(Policy = "MenuEditPolicy")]
    [HttpPost]
    public IActionResult EditCategory(MenuCategoryVM model)
    {
        Console.WriteLine("Edit name:" + model.CategoryName);
        Console.WriteLine("Edit name:" + model.CategoryId);
        Console.WriteLine("Edit name:" + model.CategoryDescription);
        // if (ModelState.IsValid)
        // {
        //     _menuService.UpdateCategory(model);
        //     return RedirectToAction("Menu", "Home");
        // }

        // return Json(new { success = false, message = "Invalid data" });
        _menuService.UpdateCategory(model);
        return RedirectToAction("Menu", "Home");
    }

    
    [Authorize(Policy = "MenuDeletePolicy")]
    [HttpPost]
    public IActionResult DeleteCategory(int id)
    {
        Console.WriteLine("tHIS IS iD: " + id);
        var category = _menuService.GetCategoryById(id);
        _menuService.DeleteCategory(category);
        TempData["Message"] = "Category deleted successfully!";
        TempData["MessageType"] = "error";
        return RedirectToAction("Menu", "Home");
    }

    
    [Authorize(Policy = "MenuDeletePolicy")]
    public IActionResult DeleteItem([FromBody] List<MenuItem> items)
    {

        Console.WriteLine("HEEJNJKFNJN");
        if (items == null || items.Count == 0)
        {
            return BadRequest("No items received");
        }

        Console.WriteLine("Updatedjfnldnfledf");
        Console.WriteLine(items.Count);

        _menuService.DeleteItem(items);

        TempData["Message"] = "Successfully Delete Item.";
        TempData["MessageType"] = "success"; // Types: success, error, warning, info



        return RedirectToAction("Menu", "Home");

    }

    
    [Authorize(Policy = "MenuEditPolicy")]
    [HttpPost]
    public IActionResult AddMenuItem(MenuCategoryVM menuItem)
    {
        Console.WriteLine(ModelState.IsValid);
        Console.WriteLine("--------------Add Category");

        if (!ModelState.IsValid)
        {
            // Log the model state errors
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Property: {state.Key}, Error: {error.ErrorMessage}");
                }
            }

        }
        var menuitem = new MenuItem
        {
            ModifierGroupId = menuItem.ModifierGroupId,
            CategoryId = menuItem.CategoryId,
            ItemName = menuItem.ItemName,
            ItemtypeId = menuItem.ItemtypeId,
            Rate = menuItem.Rate,
            Quantity = menuItem.Quantity,
            UnitId = menuItem.UnitId,
            IsAvailable = menuItem.IsAvailable,
            TaxDefault = menuItem.TaxDefault,
            TaxPercentage = menuItem.TaxPercentage,
            ShortCode = menuItem.ShortCode,
            Description = menuItem.Description
        };


        Console.WriteLine(menuitem.ItemName);



        _menuService.AddMenuItem(menuitem);

        var menuitemmodifier = new ItemModifierGroup
        {
            ItemId = menuitem.ItemId,
            ModifierGroupId = (int)menuitem.ModifierGroupId,
            MinSelection = menuItem.MinSelection,
            MaxSelection = menuItem.MaxSelection
        };

        _menuService.AddMenuItemModifierGroup(menuitemmodifier);

        Console.WriteLine("--------------Add User END");
        TempData["Message"] = "User added successfully!";
        TempData["MessageType"] = "success";


        return RedirectToAction("Menu", "Home");
    }




    // [HttpPost]
    // public IActionResult AddMenuModifierGroup(MenuModifierGroup modifierGroup)
    // {


    //     // Console.WriteLine("Hekrkjnhfrkjnfcekjnb" + categoryvm.editCategories.CategoryName);

    //     _menuService.AddModifierGroup(modifierGroup);
    //     return RedirectToAction("Menu", "Home"); 

    // }

    // [HttpPost]
    // public IActionResult AddMenuModifierGroup([FromBody] MenuModifierGroupVM model)
    // {
    //     Console.WriteLine("Received Data:");
    //     Console.WriteLine("Modifier Group Name: " + model.ModifierGroupName);
    //     Console.WriteLine("Modifier Group Description: " + model.ModifierGroupDecription);

    //     // Check if ModifierIds is null or empty
    //     if (model.ModifierIds == null || !model.ModifierIds.Any())
    //     {
    //         Console.WriteLine("No modifiers selected.");
    //     }
    //     else
    //     {
    //         Console.WriteLine("Selected Modifier IDs: " + string.Join(", ", model.ModifierIds));
    //     }

    //     // 1️⃣ Save Modifier Group
    //     var modifierGroup = new MenuModifierGroup
    //     {
    //         ModifierGroupName = model.ModifierGroupName,
    //         ModifierGroupDecription = model.ModifierGroupDecription
    //     };
    //     _menuService.AddModifierGroup(modifierGroup);

    //     Console.WriteLine("Modifier Group ID: " + modifierGroup.ModifierGroupId);

    //     // 2️⃣ Save Selected Modifiers into the Group
    //     if (model.ModifierIds != null && model.ModifierIds.Any())
    //     {
    //         foreach (var modifierId in model.ModifierIds)
    //         {
    //             var menuModifierGroup = new MenuModifier
    //             {
    //                 ModifierGroupId = modifierGroup.ModifierGroupId, // Get newly inserted ID
    //                 ModifierId = modifierId
    //             };
    //             _menuService.AddModifier(menuModifierGroup);
    //         }
    //     }

    //     return RedirectToAction("Menu", "Home");
    // }

    
    [Authorize(Policy = "MenuEditPolicy")]
    [HttpPost]
    public IActionResult AddMenuModifierGroup([FromBody] MenuModifierGroupVM model)
    {
        Console.WriteLine("Received Data:");
        Console.WriteLine("Modifier Group Name: " + model.ModifierGroupName);
        Console.WriteLine("Modifier Group Description: " + model.ModifierGroupDecription);
        Console.WriteLine("Selected Modifier IDs: " + (model.ModifierIds != null ? string.Join(", ", model.ModifierIds) : "None"));

        // 1️⃣ Save Modifier Group
        var modifierGroup = new MenuModifierGroup
        {
            ModifierGroupName = model.ModifierGroupName,
            ModifierGroupDecription = model.ModifierGroupDecription
        };

        _menuService.AddModifierGroup(modifierGroup);  // Saving to database
        Console.WriteLine("Saved Modifier Group ID: " + modifierGroup.ModifierGroupId);

        // 2️⃣ Save Selected Modifiers into the Group

        // 2️⃣ Validate and Save Selected Modifiers


        // 2️⃣ Associate Combined Modifiers (Only this step is needed)
        if (model.ModifierIds != null && model.ModifierIds.Any())
        {
            foreach (var combinedModifierId in model.ModifierIds)
            {
                var existingModifier = _menuService.GetModifierById(combinedModifierId);
                if (existingModifier != null)
                {
                    var combinedModifier = new CombineModifier
                    {
                        ModifierGroupId = modifierGroup.ModifierGroupId, // Associate with group
                        ModifierId = combinedModifierId
                    };
                    _menuService.AddCombinedModifierGroup(combinedModifier);
                    Console.WriteLine($"Linked Combined Modifier ID {combinedModifierId} to Modifier Group ID {modifierGroup.ModifierGroupId}");
                }
                else
                {
                    Console.WriteLine($"Combined Modifier with ID {combinedModifierId} not found, skipping.");
                }
            }
        }
        // 3️⃣ Return Success Response
        return RedirectToAction("Menu", "Home");
    }



    
    [Authorize(Policy = "MenuEditPolicy")]
    [HttpPost]
    public IActionResult EditModifierGroup(MenuModifierGroupVM model)
    {
        Console.WriteLine("Edit name:" + model.ModifierGroupId);
        Console.WriteLine("Edit name:" + model.ModifierGroupName);
        Console.WriteLine("Edit name:" + model.ModifierGroupDecription);

        Console.WriteLine(ModelState.IsValid);

        _menuService.UpdateModifierGroup(model);
        return RedirectToAction("Menu", "Home");



    }

    
    [Authorize(Policy = "MenuDeletePolicy")]
    [HttpPost]
    public IActionResult DeleteModifierGroup(int id)
    {
        Console.WriteLine("tHIS IS iD: " + id);
        var modifierGroup = _menuService.GetModifierGroupById(id);
        _menuService.DeleteModifierGroup(modifierGroup);
        TempData["Message"] = "Modifier Group deleted successfully!";
        TempData["MessageType"] = "error";
        return RedirectToAction("Menu", "Home");
    }

    
    [Authorize(Policy = "MenuEditPolicy")]
    public IActionResult AddMenuModifier(MenuModifierGroupVM menuModifier)
    {
        Console.WriteLine(ModelState.IsValid);
        Console.WriteLine("--------------Add Modifier");

        if (!ModelState.IsValid)
        {
            // Log the model state errors
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Property: {state.Key}, Error: {error.ErrorMessage}");
                }
            }

        }
        var menumodifier = new MenuModifier
        {
            ModifierName = menuModifier.ModifierName,
            ModifierRate = menuModifier.ModifierRate,
            Quantity = menuModifier.Quantity,
            UnitId = menuModifier.UnitId,
            ModifierGroupId = menuModifier.ModifierGroupId,
            ModifierDecription = menuModifier.ModifierDecription

        };

        Console.WriteLine(menumodifier.ModifierName);



        _menuService.AddModifier(menumodifier);
        Console.WriteLine("--------------Add Modifier END");
        TempData["Message"] = "Modifier added successfully!";
        TempData["MessageType"] = "success";


        return RedirectToAction("Menu", "Home");
    }








    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
