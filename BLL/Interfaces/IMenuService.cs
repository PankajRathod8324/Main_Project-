using System.Threading.Tasks;
using DAL.Models;
using DAL.ViewModel;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces;
public interface IMenuService
{
  IEnumerable<MenuCategory> GetAllCategories();

  MenuCategory GetCategoryById(int id);

  void AddCategory(MenuCategory category);

  void UpdateCategory(MenuCategoryVM category);

  void DeleteCategory(MenuCategory category);
  List<MenuItem> GetItemsByCategoryId(int categoryId);

  ItemModifierGroup GetItemModifier(int itemid, int modifiergroupid);
  void AddMenuItem(MenuItem menuItem);

  void AddMenuItemModifierGroup(ItemModifierGroup menuitemmodifier);

  public void UpdateMenuItem(MenuItem item);

  public MenuModifier GetModifierById(int modifierid);

  public void AddCombinedModifierGroup(CombineModifier modifierGroup);

  public void DeleteItem(List<MenuItem> items);

  public IEnumerable<Itemtype> GetAllItemTypes();

  List<MenuModifier> GetModifiers();

  public IEnumerable<Unit> GetAllUnits();

  public IEnumerable<MenuModifierGroup> GetAllModifierGroups();


  MenuModifierGroup GetModifierGroupById(int id);

  //  string GetModifierNameById(int modifierId, MenuModifierGroupVM modifierGroups);

  List<MenuModifier> GetModifiersByModifierGroupId(int modifierGroupId);

  public MenuItem GetItemById(int itemid);

  public string GetUnitById(int unitId);

  void AddModifierGroup(MenuModifierGroup modifierGroup);

  void UpdateModifierGroup(MenuModifierGroupVM modifierGroup);

  void DeleteModifierGroup(MenuModifierGroup modifierGroup);

  public void AddModifier(MenuModifier modifier);

}
