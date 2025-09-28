// When clicking Edit: load partial, show Bootstrap modal then reposition the modal-dialog
$(function () {
    $(".edit-film").on("click", function () {
        const filmId = $(this).data("id");
        $.get("/Film/Edit/" + filmId, function (html) {
            $("#filmModalContent").html(html);
            $("#filmModal").modal("show");
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

$(document).on("click", ".edit-film", function () {
    var filmId = $(this).data("id");

    $("#filmModalContent").load("/Films/Edit/" + filmId, function () {
        var modalEl = document.getElementById('filmModal');
        var bsModal = new bootstrap.Modal(modalEl, { backdrop: "static" });
        bsModal.show();
    });
});
