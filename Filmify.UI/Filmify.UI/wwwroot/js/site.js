// وقتی روی Edit کلیک می‌کنیم
$(document).on("click", ".edit-film", function () {
    var filmId = $(this).data("id");

    $("#filmModalContent").load("/Films/Edit/" + filmId, function () {
        var modalEl = document.getElementById('filmModal');
        var bsModal = new bootstrap.Modal(modalEl, { backdrop: "static" });
        bsModal.show();

        // اینجا مقداردهی اولیه hidden input
        updateHiddenTags();
    });
});

// اضافه کردن و حذف تگ
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

// submit فرم
$("#editFilmForm").on("submit", function () {
    updateHiddenTags();
});

// تابع برای بروزرسانی hidden input
function updateHiddenTags() {
    let ids = [];
    $("#tagContainer .tag-badge").each(function () {
        ids.push($(this).data("id"));
    });
    $("#TagIdsHidden").val(ids.join(","));
}
