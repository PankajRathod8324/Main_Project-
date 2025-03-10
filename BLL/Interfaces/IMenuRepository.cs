using System.Threading.Tasks;
using DAL.Models;
using DAL.ViewModel;

namespace BLL.Interfaces;
public interface IMenuRepository
{

    IEnumerable<MenuCategory> GetAllCategories();

    MenuCategory GetCategoryById(int id);

    void AddCategory(MenuCategory category);

    void UpdateCategory(MenuCategoryVM category);

    void DeleteCategory(MenuCategory category);
    List<MenuItem> GetItemsByCategoryId(int categoryId);
    void AddMenuItem(MenuItem menuItem);

    void AddMenuItemModifierGroup(ItemModifierGroup menuitemmodifier);

    public void UpdateMenuItem(MenuItem item);

    public void DeleteItems(List<MenuItem> items);

    public IEnumerable<Itemtype> GetAllItemTypes();


    List<MenuModifier> GetModifiers();
    public IEnumerable<Unit> GetAllUnits();

    public IEnumerable<MenuModifierGroup> GetAllModifierGroups();


    MenuModifierGroup GetModifierGroupById(int id);

     public void AddCombinedModifierGroup(CombineModifier modifierGroup);

      public MenuModifier GetModifierById(int modifierid);

    //  string GetModifierNameById(int modifierId, MenuModifierGroupVM modifierGroups);

    public MenuItem GetItemById(int itemid);

    ItemModifierGroup GetItemModifier(int itemid, int modifiergroupid);

    public string GetUnitById(int unitId);

    List<MenuModifier> GetModifiersByModifierGroupId(int modifierGroupId);

    void AddModifierGroup(MenuModifierGroup modifierGroup);

    void UpdateModifierGroup(MenuModifierGroupVM modifierGroup);

    void DeleteModifierGroup(MenuModifierGroup modifierGroup);

    public void AddModifier(MenuModifier modifier);


}