using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public class RecipeList : ListView<Ricetta>
{
    public RecipeList()
    {
        var template = new RecipeFull();

        Template = template;
    }
}