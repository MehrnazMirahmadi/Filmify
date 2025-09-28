// When clicking Edit: load partial, show Bootstrap modal then reposition the modal-dialog
$(document).on("click", ".edit-film", function () {
    var filmId = $(this).data("id");
    var $button = $(this);

    // Load partial into modal content
    $("#filmModalContent").load("/Films/Edit/" + filmId, function () {
        var modalEl = document.getElementById('filmModal');
        var $modal = $(modalEl);
        var dialog = $modal.find(".modal-dialog")[0];

        // Remove any centering class to avoid Bootstrap vertical centering
        $(dialog).removeClass("modal-dialog-centered");

        // Create and show the modal
        var bsModal = new bootstrap.Modal(modalEl, { backdrop: true });
        bsModal.show();

        // After the modal is visible we can measure and position it
        $modal.one('shown.bs.modal', function () {
            // Measure button and dialog
            var buttonRect = $button[0].getBoundingClientRect();
            var dialogRect = dialog.getBoundingClientRect();

            // Center horizontally over the table row/button, and vertically align center to the button
            var top = buttonRect.top + (buttonRect.height / 2) - (dialogRect.height / 2);
            var left = buttonRect.left + (buttonRect.width / 2) - (dialogRect.width / 2);

            // Constrain within viewport with small padding
            var padding = 8;
            top = Math.max(padding, Math.min(top, window.innerHeight - dialogRect.height - padding));
            left = Math.max(padding, Math.min(left, window.innerWidth - dialogRect.width - padding));

            // Apply fixed positioning to place the modal over the table
            dialog.style.position = 'fixed';
            dialog.style.top = top + 'px';
            dialog.style.left = left + 'px';
            dialog.style.margin = '0'; // ensure no margins interfere
            dialog.style.transform = 'none'; // avoid Bootstrap transforms
        });

        // Reset dialog positioning when modal hides to avoid leftover inline styles
        $modal.one('hidden.bs.modal', function () {
            dialog.style.position = '';
            dialog.style.top = '';
            dialog.style.left = '';
            dialog.style.margin = '';
            dialog.style.transform = '';
            // clear loaded content to avoid stale listeners
            $("#filmModalContent").empty();
        });
    });
});

function updateHiddenTags() {
    let ids = [];
    $("#tagContainer .tag-badge").each(function () {
        ids.push($(this).data("id"));
    });
    $("#TagIdsHidden").val(ids.join(","));
}

$(document).on("click", "#addTagBtn", function () {
    let newTag = $("#newTagInput").val().trim();
    if (newTag) {
        let newId = "new_" + newTag.replace(/\s+/g, "_"); // 
        $("#tagContainer").append('<span class="badge bg-primary tag-badge" data-id="' + newId + '">' + newTag + ' <a href="#" class="text-white remove-tag">&times;</a></span>');
        $("#newTagInput").val("");
        updateHiddenTags();
    }
});

$(document).on("click", ".remove-tag", function (e) {
    e.preventDefault();
    $(this).parent().remove();
    updateHiddenTags();
});


$("#editFilmForm").on("submit", function () {
    updateHiddenTags();
});