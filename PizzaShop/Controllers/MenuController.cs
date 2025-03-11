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
    public IActionResult MenuItem(int categoryId, UserFilterOptions filterOptions)
    {
        filterOptions.Page ??= 1;
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


        Console.WriteLine("Click this here : " + categoryId);
        var unitName = _menuService.GetAllUnits().ToList();
        // categoryId = categoryId ? null : 11;


        X.PagedList.IPagedList<MenuCategoryVM> menuitemvm = _menuService.getFilteredMenuItems(categoryId, filterOptions);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_MenuItemPV", menuitemvm);
        }
        return PartialView("_MenuItemPV", menuitemvm);
    }


    [Authorize(Policy = "MenuEditPolicy")]
    public IActionResult EditMenuItem(int itemId)
    {
        // Fetch Categories
        ViewBag.Categories = _menuService.GetAllCategories()
            .Select(r => new SelectListItem { Value = r.CategoryId.ToString(), Text = r.CategoryName })
            .ToList();

        // Fetch Item Types
        ViewBag.Itemtypes = _menuService.GetAllItemTypes()
            .Select(r => new SelectListItem { Value = r.ItemtypeId.ToString(), Text = r.ItemType1 })
            .ToList();

        // Fetch Units
        ViewBag.Units = _menuService.GetAllUnits()
            .Select(r => new SelectListItem { Value = r.UnitId.ToString(), Text = r.UnitName })
            .ToList();

        // Fetch Modifier Groups
        ViewBag.ModifierGroups = _menuService.GetAllModifierGroups()
            .Select(r => new SelectListItem { Value = r.ModifierGroupId.ToString(), Text = r.ModifierGroupName })
            .ToList();

        // Get the Item
        var item = _menuService.GetItemById(itemId);
        if (item == null)
        {
            return NotFound(); // Return a 404 if the item doesn't exist
        }

        // Fetch associated modifier groups
        ItemModifierGroup itemModifiers = _menuService.GetItemModifier(item.ItemId, (int)item.ModifierGroupId);

        var itemvm = new MenuCategoryVM
        {
            CategoryId = item.CategoryId,
            ItemId = item.ItemId,
            ItemName = item.ItemName,
            ItemtypeId = item.ItemtypeId,
            Rate = item.Rate,
            Quantity = item.Quantity,
            UnitId = item.UnitId,
            IsAvailable = item.IsAvailable ?? false,
            TaxDefault = item.TaxDefault,
            TaxPercentage = item.TaxPercentage,
            ShortCode = item.ShortCode,
            Description = item.Description,
            ModifierGroupId = item.ModifierGroupId
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

        var modifier = _menuService.GetItemModifier(model.ItemId, (int)model.ModifierGroupId);

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
    public IActionResult MenuModifier(int groupId, UserFilterOptions filterOptions)
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


        Console.WriteLine("Click this : " + groupId);
        filterOptions.Page ??= 1;

        X.PagedList.IPagedList<MenuModifierGroupVM> menumodifiervm = _menuService.getFilteredMenuModifiers(groupId, filterOptions);


        // if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        // {
        //     return PartialView("_MenuModifierPV", menumodifiervm);
        // }


        // MenuModifierGroupVM modifiervm = new MenuModifierGroupVM
        // {
        //     menuModifiers = menumodifiervm
        //     // UnitName = modifiers.UnitId.HasValue ? _menuService.GetUnitById(modifiers.UnitId.Value) : "No Units"
        // };
        return PartialView("_MenuModifierPV", menumodifiervm);
    }

    [Authorize]
    public IActionResult GetAllModifier()
    {
        var modifiers = _menuService.GetModifiers();

        if (modifiers == null || !modifiers.Any())
        {
            Console.WriteLine("❌ No Modifiers Found!");
            return NotFound("No modifiers found.");
        }

        Console.WriteLine("✅ Modifiers Retrieved:");
        foreach (var modifier in modifiers)
        {
            Console.WriteLine(modifier.ModifierName);
        }

        var modifierItems = modifiers.Select(item => new MenuModifierGroupVM
        {
            ModifierGroupId = item.ModifierGroupId ?? 0, // Avoid null exception
            ModifierId = item.ModifierId,
            ModifierName = item.ModifierName,
            ModifierRate = item.ModifierRate,
            UnitId = item.UnitId,
            Quantity = item.Quantity,
            ModifierDecription = item.ModifierDecription,
            UnitName = item.UnitId.HasValue ? (_menuService.GetUnitById(item.UnitId.Value) ?? "No Unit") : "No Unit"
        }).ToList();

        var modifierVM = new MenuModifierGroupVM
        {
            menuModifiers = modifierItems
        };

        return PartialView("_EditModifierByModifierGroup.", modifierVM);
    }

    [Authorize]
    public IActionResult GetModifiersByGroup(int groupId, UserFilterOptions filterOptions)
    {
        var modifiers = _menuService.GetModifiersByModifierGroupId(groupId);
        var groupName = _menuService.GetModifierGroupById(groupId);

        var modifieritems = modifiers
           .Select(item => new MenuModifierGroupVM
           {
               ModifierGroupId = (int)item.ModifierGroupId,
               ModifierName = item.ModifierName,
               ModifierRate = item.ModifierRate,
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
        // if (!ModelState.IsValid)
        // {
        //     foreach (var state in ModelState)
        //     {
        //         foreach (var error in state.Value.Errors)
        //         {
        //             Console.WriteLine($"Property: {state.Key}, Error: {error.ErrorMessage}");
        //         }
        //     }
        //     return View(menuItem);
        // }

        // Step 1: Save Menu Item
        var menuitem = new MenuItem
        {
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

        _menuService.AddMenuItem(menuitem);

        // Step 2: Save Multiple Modifier Groups
        if (menuItem.ModifierGroupIds != null && menuItem.ModifierGroupIds.Any())
        {
            foreach (var modifierGroupId in menuItem.ModifierGroupIds)
            {
                var menuitemmodifier = new ItemModifierGroup
                {
                    ItemId = menuitem.ItemId, // Link to menu item
                    ModifierGroupId = modifierGroupId.ModifierGroupId,
                    MinSelection = menuItem.MinSelection,
                    MaxSelection = menuItem.MaxSelection
                };

                _menuService.AddMenuItemModifierGroup(menuitemmodifier);
            }
        }

        TempData["Message"] = "Menu item added successfully!";
        TempData["MessageType"] = "success";

        return RedirectToAction("MenuModifier", "Menu", menuitem.ModifierGroupId);
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

    [HttpGet]
    public IActionResult GetModifierGroup(int id)
    {
        Console.WriteLine($"Fetching Modifier Group for ID: {id}");

        var group = _menuService.GetModifierGroupById(id);
        if (group == null)
        {
            Console.WriteLine("❌ Modifier Group not found!");
            return NotFound("Modifier Group not found.");
        }

        var modifiers = _menuService.GetModifiersByModifierGroupId(id);

        Console.WriteLine($"✅ Found {modifiers.Count()} Modifiers in Group {id}");

        var viewModel = new MenuModifierGroupVM
        {
            ModifierGroupId = group.ModifierGroupId,
            ModifierGroupName = group.ModifierGroupName,
            ModifierGroupDecription = group.ModifierGroupDecription,
            ModifierIds = modifiers.Select(m => m.ModifierId).ToList()
        };

        return PartialView("_EditModifierGroupPV", viewModel); // Return Partial View

    }



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
    public IActionResult EditModifierGroup([FromBody] MenuModifierGroupVM model)
    {
        if (model == null)
        {
            return Json(new { success = false, message = "Invalid data received." });
        }

        Console.WriteLine("Editing Modifier Group ID:" + model.ModifierGroupId);
        Console.WriteLine("New Name:" + model.ModifierGroupName);
        Console.WriteLine("New Description:" + model.ModifierGroupDecription);

        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Invalid data." });
        }

        // Fetch existing Modifier Group from DB
        var existingGroup = _menuService.GetModifierGroupById(model.ModifierGroupId);
        if (existingGroup == null)
        {
            return Json(new { success = false, message = "Modifier Group not found." });
        }

        // Update basic details
        existingGroup.ModifierGroupName = model.ModifierGroupName;
        existingGroup.ModifierGroupDecription = model.ModifierGroupDecription;

        // Update Modifier Group associations
        var existingModifierIds = _menuService.GetModifiersByModifierGroupId(model.ModifierGroupId)
                                                .Select(m => m.ModifierId)
                                                .ToList();

        // Find modifiers to remove (Old ones that are not in the new list)
        var modifiersToRemove = existingModifierIds.Except(model.ModifierIds).ToList();
        foreach (var modifierId in modifiersToRemove)
        {
            // _menuService.RemoveModifierFromGroup(model.ModifierGroupId, modifierId);
        }

        // Find modifiers to add (New ones that were not in the old list)
        var modifiersToAdd = model.ModifierIds.Except(existingModifierIds).ToList();
        foreach (var modifierId in modifiersToAdd)
        {
            var combinedModifier = new CombineModifier
            {
                ModifierId = modifierId,
                ModifierGroupId = model.ModifierGroupId
            };
            _menuService.AddCombinedModifierGroup(combinedModifier);
        }

        // _menuService.UpdateModifierGroup(existingGroup);

        return Json(new { success = true, message = "Modifier Group updated successfully!" });
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
        // Console.WriteLine("--------------Add Modifier" + menuModifier.ModifierGroupId);


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


        var newmenumodifier = new MenuModifier
        {
            ModifierName = menuModifier.ModifierName,
            ModifierRate = menuModifier.ModifierRate,
            Quantity = menuModifier.Quantity,
            UnitId = menuModifier.UnitId,
            ModifierDecription = menuModifier.ModifierDecription

        };

        _menuService.AddModifier(newmenumodifier);

        // Add multiple Modifier Groups
        if (menuModifier.ModifierGroupIds != null && menuModifier.ModifierGroupIds.Any())
        {
            foreach (var groupId in menuModifier.ModifierGroupIds)
            {
                var combinedModifier = new CombineModifier
                {
                    ModifierId = newmenumodifier.ModifierId,
                    ModifierGroupId = groupId
                };
                _menuService.AddCombinedModifierGroup(combinedModifier);
            }
        }

        Console.WriteLine("--------------Add Modifier END");
        TempData["Message"] = "Modifier added successfully!";
        TempData["MessageType"] = "success";



        return Json(new { success = true, message = "Modifier Added Successfully!" });
    }




    public IActionResult EditMenuModifier(int modifierid)
    {
        Console.WriteLine(modifierid);

        // Fetch Units from the database
        var units = _menuService.GetAllUnits();
        ViewBag.Units = units.Select(r => new SelectListItem
        {
            Value = r.UnitId.ToString(),
            Text = r.UnitName
        }).ToList();

        // Fetch Modifier Groups from the database
        var modifierGroups = _menuService.GetAllModifierGroups();
        ViewBag.ModifierGroups = modifierGroups.Select(r => new SelectListItem
        {
            Value = r.ModifierGroupId.ToString(),
            Text = r.ModifierGroupName
        }).ToList();

        // Fetch the modifier item
        var modifier = _menuService.GetModifierById(modifierid);
        if (modifier == null)
        {
            return NotFound();
        }

        // Fetch associated modifier group IDs (multiple)
        var assignedModifierGroups = _menuService.GetModifierGroupsByModifierId(modifierid);

        var itemvm = new MenuModifierGroupVM
        {
            ModifierId = modifier.ModifierId,
            ModifierGroupIds = assignedModifierGroups.Select(mg => mg.ModifierGroupId).ToList(), // Multiple selection
            ModifierName = modifier.ModifierName,
            ModifierRate = modifier.ModifierRate,
            UnitId = modifier.UnitId,
            Quantity = modifier.Quantity,
            ModifierDecription = modifier.ModifierDecription,
            UnitName = modifier.UnitId.HasValue ? _menuService.GetUnitById(modifier.UnitId.Value) : "No Unit"
        };

        return PartialView("_EditModifierPV", itemvm);
    }

    [HttpPost]
    [Authorize(Policy = "MenuEditPolicy")]
    public IActionResult EditMenuModifier([FromBody] MenuModifierGroupVM menuModifier)
    {
        if (menuModifier == null)
        {
            return Json(new { success = false, message = "Invalid request data." });
        }

        Console.WriteLine(ModelState.IsValid);

        // Fetch the existing modifier
        var existingModifier = _menuService.GetModifierById(menuModifier.ModifierId);
        if (existingModifier == null)
        {
            return Json(new { success = false, message = "Modifier not found." });
        }

        // Update the modifier properties
        existingModifier.ModifierName = menuModifier.ModifierName;
        existingModifier.ModifierRate = menuModifier.ModifierRate;
        existingModifier.Quantity = menuModifier.Quantity;
        existingModifier.UnitId = menuModifier.UnitId;
        existingModifier.ModifierDecription = menuModifier.ModifierDecription;

        _menuService.UpdateModifier(existingModifier);

        // Fetch existing assigned modifier groups
        var existingModifierGroups = _menuService.GetModifierGroupsByModifierId(existingModifier.ModifierId)
                                                  .Select(mg => mg.ModifierGroupId)
                                                  .ToList();

        var newModifierGroups = menuModifier.ModifierGroupIds ?? new List<int>();

        // Find groups to remove (exist in old but not in new)
        var groupsToRemove = existingModifierGroups.Except(newModifierGroups).ToList();
        // Find groups to add (exist in new but not in old)
        var groupsToAdd = newModifierGroups.Except(existingModifierGroups).ToList();

        // Remove modifier groups that are no longer selected
        foreach (var groupId in groupsToRemove)
        {
            _menuService.RemoveCombinedModifierGroup(existingModifier.ModifierId, groupId);
        }

        // Add new modifier groups that were selected
        foreach (var groupId in groupsToAdd)
        {
            var combinedModifier = new CombineModifier
            {
                ModifierId = existingModifier.ModifierId,
                ModifierGroupId = groupId
            };
            _menuService.AddCombinedModifierGroup(combinedModifier);
        }

        Console.WriteLine("--------------Edit Modifier END");
        TempData["Message"] = "Modifier updated successfully!";
        TempData["MessageType"] = "success";

        return Json(new { success = true, message = "Modifier updated successfully!" });
    }



    public IActionResult DeleteModifier([FromBody] List<MenuModifier> modifiers)
    {
        Console.WriteLine("HEEJNJKFNJN");
        if (modifiers == null || modifiers.Count == 0)
        {
            return BadRequest("No modifiers received");
        }

        Console.WriteLine("Updatedjfnldnfledf");
        Console.WriteLine(modifiers.Count);

        _menuService.DeleteModifiers(modifiers);

        TempData["Message"] = "Successfully Delete Item.";
        TempData["MessageType"] = "success"; // Types: success, error, warning, info



        return RedirectToAction("Menu", "Home");
    }







    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}