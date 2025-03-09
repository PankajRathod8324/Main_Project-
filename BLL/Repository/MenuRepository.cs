using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repository;

public class MenuRepository : IMenuRepository
{
    private readonly PizzaShopContext _context;

    public MenuRepository(PizzaShopContext context)
    {
        _context = context;
    }

    public IEnumerable<MenuCategory> GetAllCategories()
    {
        return _context.MenuCategories.Where(c => (bool)!c.IsDeleted).ToList();
    }

    public MenuCategory GetCategoryById(int id)
    {
        return _context.MenuCategories.Include(c => c.MenuItems)
                                            .FirstOrDefault(c => c.CategoryId == id);
    }

    public void AddCategory(MenuCategory category)
    {
        _context.MenuCategories.Add(category);
        Save();
    }

    public void UpdateCategory(MenuCategoryVM category)
    {
        var id = _context.MenuCategories.FirstOrDefault(u => u.CategoryId == category.CategoryId);
        id.CategoryDescription = category.CategoryDescription;
        id.CategoryName = category.CategoryName;
        Save();
    }

    public void DeleteCategory(MenuCategory category)
    {
        Console.WriteLine(category);
        if (category != null)
        {
            category.IsDeleted = true;
            Save();
        }
    }

    public List<MenuItem> GetItemsByCategoryId(int categoryId)
    {
        return _context.MenuItems.Where(i => i.CategoryId == categoryId && i.IsDeleted == (bool)!i.IsDeleted).ToList();
    }

    public void AddMenuItem(MenuItem menuItem)
    {
        _context.MenuItems.Add(menuItem);
        _context.SaveChanges();

    }

    public void AddMenuItemModifierGroup(ItemModifierGroup menuitemmodifier)
    {
        _context.ItemModifierGroups.Add(menuitemmodifier);
        _context.SaveChanges();
    }


    public void UpdateMenuItem(MenuItem item)
    {
        _context.MenuItems.Update(item);
        Save();
    }

    public void DeleteItems(List<MenuItem> items)
    {
        Console.WriteLine(items);
        foreach (var p in items)
        {
            Console.WriteLine($"itemsId:{p.ItemId}");
        }

        foreach (var pr in items)
        {
            pr.IsDeleted = true;
        }

        Save();
    }


    public List<MenuModifier> GetModifiers()
    {
        return _context.MenuModifiers.ToList();
    }


    public IEnumerable<Itemtype> GetAllItemTypes()
    {
        return _context.Itemtypes.ToList();
    }


    public IEnumerable<Unit> GetAllUnits()
    {
        return _context.Units.ToList();
    }

    public IEnumerable<MenuModifierGroup> GetAllModifierGroups()
    {
        return _context.MenuModifierGroups.ToList();
    }

    public MenuModifierGroup GetModifierGroupById(int id)
    {
        return _context.MenuModifierGroups.Include(c => c.MenuModifiers)
                                            .FirstOrDefault(c => c.ModifierGroupId == id);
    }

    // public string GetModifierGroupById(int modifierId)
    // {
    //     var groupName = (from modifier in _context.MenuModifiers
    //                     join groups in _context.MenuModifierGroups on modifier.ModifierGroupId equals groups.ModifierGroupId
    //                     where modifier.ModifierGroupId == modifierId
    //                     select groups.ModifierGroupName).FirstOrDefault();
    //     Console.WriteLine(groupName);
    //     return groupName;
    // }



    public List<MenuModifier> GetModifiersByModifierGroupId(int modifierGroupId)
    {
        return _context.MenuModifiers.Where(i => i.ModifierGroupId == modifierGroupId).ToList();
    }

    public string GetUnitById(int unitId)
    {
        var unitName = (from item in _context.MenuItems
                        join unit in _context.Units on item.UnitId equals unit.UnitId
                        select unit.UnitName).FirstOrDefault();
        return unitName;
    }

    public MenuItem GetItemById(int itemid)
    {
        return _context.MenuItems.FirstOrDefault(r => r.ItemId == itemid);
    }

    public MenuModifier GetModifierById(int modifierid)
    {
        return _context.MenuModifiers.FirstOrDefault(r => r.ModifierId == modifierid);
    }

    public ItemModifierGroup GetItemModifier(int itemid, int modifiergroupid)
    {
        return _context.ItemModifierGroups.FirstOrDefault(r => r.ItemId == itemid && r.ModifierGroupId == modifiergroupid);
    }


    public void AddModifierGroup(MenuModifierGroup modifierGroup)
    {
        _context.MenuModifierGroups.Add(modifierGroup);
        Save();
    }

    public void AddCombinedModifierGroup(CombineModifier modifierGroup)
    {
        _context.CombineModifiers.Add(modifierGroup);
        Save();
    }

    public void AddModifier(MenuModifier modifier)
    {
        _context.MenuModifiers.Add(modifier);
        Save();
    }

    public void UpdateModifierGroup(MenuModifierGroupVM modifierGroup)
    {
        var id = _context.MenuModifierGroups.FirstOrDefault(u => u.ModifierGroupId == modifierGroup.ModifierGroupId);
        id.ModifierGroupName = modifierGroup.ModifierGroupName;
        id.ModifierGroupDecription = modifierGroup.ModifierGroupDecription;
        Save();
    }




    public void DeleteModifierGroup(MenuModifierGroup modifierGroup)
    {
        Console.WriteLine(modifierGroup);
        if (modifierGroup != null)
        {
            modifierGroup.IsDeleted = true;
            Save();
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }



}