function AddNewIngredient(recipeId) {
    var url = "/Recipes/AddNewIngredient/" + recipeId;
    $.ajax({
        async: false,
        url: url,
        method: "GET"
    }).success(function (partialView) {
        // Set the sort order based on existing items on the page
        var maxNumber = 0;
        $('.ingredientsContainer').find('input.number').each(function() {
            var number = parseInt($(this).val());
            if(number > maxNumber) {
                maxNumber = number;
            }
        });

        // See Recipes/Create.cshtml, we need to wrap the response so it's movable
        var wrapper = $('<div class="ingredientWrapper"></div>').append(partialView);
        $('.ingredientsContainer').append(wrapper);
        $(wrapper).find('input.number').val(maxNumber + 1);

        wireOrderingLinks();
        wireRemoveLinks();
    });
}
