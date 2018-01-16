// With jQuery, you have to wrangle behavior pretty intentionally:
// - You have to globally tell everything how to behave - that's why this exists in a file rather than being inline in the editor code
// - If you add a new component, you need to tell it how to behave. It doesn't have any sort of default behavior.
// - When running mutation code like moving/deleting/new-ing, you need to re-evaluate behavior on every mutation. For instance,
//   if you moved an element up in the list, you need to tell the up arrow to no longer display, and then you need to tell the up
//   arrow for the formerly-first item to display, then you need to re-evaluate down arrows because the list may be too long and
//   you might need to hide the down arrow for the formerly-top element.
function wireOrderingLinks() {
    // Grab each "up" anchor tag which exists in the ingredients outer container
    $('div.ingredientsContainer')
    .find('a.moveUp')
    .each(function() {
        // We may be changing behavior in the case of added/removed elements,
        // so we need to remove existing behavior before assigning anything new
        $(this).unbind('click');
        // Grab the top-level wrapper for the ingredient, this is what we'll move around
        var ingredient = $(this).parents('div.ingredientWrapper');
        // Find the previous top-level ingredient wrapper
        var previous = $(ingredient).prev();
        // Determine whether we're at the beginning of the list
        if($(previous).length > 0) {
            // This item may have been hidden before, but doesn't need to be now
            // because we're not at the top of the list
            $(this).removeClass('hidden');
            // Configure the tag to do the behavior we want
            wirePreviousLink(this);
        } else {
            // We're at the beginning of the list, there's no behavior to configure.
            $(this).addClass('hidden');
        }
    });

    // This does the same thing as the previous code, but for the 'next' behavior.
    // There's a way to unify them into a single chunk of code, but it's more complex
    // than the refactor is worth.
    $('div.ingredientsContainer')
    .find('a.moveDown')
    .each(function() {
        var ingredient = $(this).parents('div.ingredientWrapper');
        var next = $(ingredient).next();
        $(this).unbind('click');
        if($(next).length > 0) {
            $(this).removeClass('hidden');
            wireNextLink(this);
        } else {
            $(this).addClass('hidden');
        }
    });
}

// The "Remove" anchor tag was originally inline javascript bound to whether or not
// the ingredient was the first in the list. Since we're moving ingredients around,
// we now have to re-evaluate what it means - I've updated it to show the "remove"
// behavior for ingredient lists greater than 1
function wireRemoveLinks() {   
    // Find all "remove links"
    var removeLinks = $('div.ingredientsContainer').find('a.remove');
    // Create a blank slate for behaviors
    $(removeLinks).unbind('click');
    // If there are more than one ingredients, wire up behavior
    if($(removeLinks).length > 1) {
        $(removeLinks).each(function() {
            // Make sure the link is shown
            $(this).removeClass('hidden');
            // Wire up behavior. In the up/down links, I refactored this into its own
            // method. The code is simpler here so I just kept it inline
            $(this).on('click', function() {
                // Brief tangent: in component moving/adding/removing, it's important
                // to encapsulate components within a single parent div. That way you
                // can just move/remove it all in a single pass. This removes the top-
                // level wrapper for an ingredient
                $(this).parents('div.ingredientWrapper').remove();
                // Now that the item has been removed, we need to call the wireup method
                // again because we might have crossed the boundary from 2 items to 1, and
                // the link might need to be hidden
                wireRemoveLinks();
                // We also need to rewire the ordering methods because we might have removed
                // the top or bottom item from the list, and the arrow behavior needs to be
                // re-evaluated.
                // ----
                // Brief tangent, it's important to note that all of this code is event-driven.
                // So while this appears to be an infinitely recursive call, the code block that
                // this is in [$(this).on('click', function() {] is only called when the anchor
                // tag is clicked. So it's not actually recursive, it just gets re-called every
                // time the event is triggered.
                wireOrderingLinks();
            });
        });
    // If there is only one ingredient, there's no behavior
    } else {
        $(removeLinks).each(function() {
            $(this).addClass('hidden');
        });
    }
}

function wirePreviousLink(anchorTag) {
    // The anchor element is passed in, this chunk of code attaches
    // custom behavior to what happens when it's clicked
    $(anchorTag).on('click', function() {
        // Grab the high-level component encapsulating the ingredient
        var ingredient = $(this).parents("div.ingredientWrapper");
        // Grab the previous ingredient, we're going to switch these
        var previous = $(ingredient).prev();
        // Do the switcheroo
        $(ingredient).remove();
        $(ingredient).insertBefore(previous);

        // We might have changed the top/bottom ingredient, so we need
        // to re-evaluate ordering links
        wireOrderingLinks();
    });
}

// Same thing as previous behavior, just in reverse
function wireNextLink(anchorTag) {
    $(anchorTag).on('click', function() {
        var ingredient = $(this).parents("div.ingredientWrapper");
        var next = $(ingredient).next();
        $(ingredient).remove();
        $(ingredient).insertAfter(next);

        wireOrderingLinks();
    });
}

// This is called when the page loads. We need to handle that base
// case for setting up behavior.
$(document).ready(function() {
    wireOrderingLinks();
    wireRemoveLinks();
});

