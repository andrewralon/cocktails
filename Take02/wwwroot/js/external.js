function AddNewIngredient(recipeId) {
    var url = "/Recipes/AddNewIngredient/" + recipeId;
    $.ajax({
        async: false,
        url: url,
        method: "GET"
    }).success(function (partialView) {
        $('#newIngredient').append(partialView);
    });
}
