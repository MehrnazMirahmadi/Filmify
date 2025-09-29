//  Edit 
$(document).on("click", ".edit-film", function () {
    var filmId = $(this).data("id");

    $("#filmModalContent").load("/Films/Edit/" + filmId, function () {
        var modalEl = document.getElementById('filmModal');
        var bsModal = new bootstrap.Modal(modalEl, { backdrop: "static" });
        bsModal.show();

        //  hidden input
        updateHiddenTags();
    });
});

$(document).on("click", "#addNewFilmBtn", function () {
    $("#filmModalContent").load("/Films/Create", function () {
        var modalEl = document.getElementById('filmModal');
        var bsModal = new bootstrap.Modal(modalEl, { backdrop: "static" });
        bsModal.show();

       
        updateHiddenTags();
    });
});

// 
$(document).on("click", "#addTagBtn", function () {
    let newTag = $("#newTagInput").val().trim();
    if (newTag) {
        let newId = "new_" + newTag.replace(/\s+/g, "_");
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

// submit 
$("#editFilmForm").on("submit", function () {
    updateHiddenTags();
});

// update hidden input
function updateHiddenTags() {
    let ids = [];
    $("#tagContainer .tag-badge").each(function () {
        ids.push($(this).data("id"));
    });
    $("#TagIdsHidden").val(ids.join(","));
}
// AJAX submit 
$(document).on("submit", "#createFilmForm", function (e) {
    e.preventDefault();
    updateHiddenTags();

    var formData = new FormData(this);

    $.ajax({
        url: '/Films/Create',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                $("#filmModal").modal('hide');
                location.reload(); // 
            } else {
                alert(response.message);
            }
        },
        error: function (err) {
            console.error(err);
            alert("Error saving film.");
        }
    });
});