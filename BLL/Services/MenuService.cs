using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using DAL.ViewModel;

namespace BLL.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    public MenuService(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }



    public IEnumerable<MenuCategory> GetAllCategories()
    {
        return _menuRepository.GetAllCategories();
    }

    public MenuCategory GetCategoryById(int id)
    {
        return _menuRepository.GetCategoryById(id);
    }

    public void AddCategory(MenuCategory category)
    {
        _menuRepository.AddCategory(category);
    }

    public void UpdateCategory(MenuCategoryVM category)
    {
        _menuRepository.UpdateCategory(category);
    }

    public void DeleteCategory(MenuCategory category)
    {
        _menuRepository.DeleteCategory(category);
    }

    public List<MenuItem> GetItemsByCategoryId(int categoryId)
    {
        return _menuRepository.GetItemsByCategoryId(categoryId);
    }

    public ItemModifierGroup GetItemModifier(int itemid, int modifiergroupid)
    {
        return _menuRepository.GetItemModifier(itemid, modifiergroupid);
    }

    public void AddMenuItem(MenuItem menuItem)
    {
        _menuRepository.AddMenuItem(menuItem);
    }

    public void AddMenuItemModifierGroup(ItemModifierGroup menuitemmodifier)
    {
        _menuRepository.AddMenuItemModifierGroup(menuitemmodifier);
    }

    public void UpdateMenuItem(MenuItem item)
    {
        _menuRepository.UpdateMenuItem(item);
    }

    public void DeleteItem(List<MenuItem> items)
    {
        _menuRepository.DeleteItems(items);
    }


    public IEnumerable<Itemtype> GetAllItemTypes()
    {
        return _menuRepository.GetAllItemTypes();
    }

    public IEnumerable<Unit> GetAllUnits()
    {
        return _menuRepository.GetAllUnits();
    }

    public IEnumerable<MenuModifierGroup> GetAllModifierGroups()
    {
        return _menuRepository.GetAllModifierGroups();
    }
    public MenuModifierGroup GetModifierGroupById(int id)
    {
        return _menuRepository.GetModifierGroupById(id);
    }

    // public string GetModifierNameById(int modifierId, MenuModifierGroupVM modifierGroups)
    // {
    //     return _menuRepository.GetModifierNameById(modifierId, modifierGroups);
    // }

    public List<MenuModifier> GetModifiers()
    {
        return _menuRepository.GetModifiers();
    }
    public string GetUnitById(int unitId)
    {
        return _menuRepository.GetUnitById(unitId);
    }

    public MenuModifier GetModifierById(int modifierid)
    {
        return _menuRepository.GetModifierById(modifierid);
    }


    public void AddCombinedModifierGroup(CombineModifier modifierGroup)
    {
        _menuRepository.AddCombinedModifierGroup(modifierGroup);
    }
    public MenuItem GetItemById(int itemid)
    {
        return _menuRepository.GetItemById(itemid);
    }
    public List<MenuModifier> GetModifiersByModifierGroupId(int modifierGroupId)
    {
        return _menuRepository.GetModifiersByModifierGroupId(modifierGroupId);
    }

    public void AddModifierGroup(MenuModifierGroup modifierGroup)
    {
        _menuRepository.AddModifierGroup(modifierGroup);
    }

    public void UpdateModifierGroup(MenuModifierGroupVM modifierGroup)
    {
        _menuRepository.UpdateModifierGroup(modifierGroup);
    }

    public void DeleteModifierGroup(MenuModifierGroup modifierGroup)
    {
        _menuRepository.DeleteModifierGroup(modifierGroup);
    }

    public void AddModifier(MenuModifier modifier)
    {
        _menuRepository.AddModifier(modifier);
    }

}
