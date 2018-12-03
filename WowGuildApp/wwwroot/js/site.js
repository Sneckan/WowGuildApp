$(".user-profile").click(function () {
    var user = $(this).attr("data-user");
    var url = '/Profile/' + user;
    window.location.href = url;
});

$('.reply-with-quote').click(function () {
    var poster = $(this).closest(".post-comment").find(".user-username").html();
    var text = $(this).closest(".post-comment").find(".post-content");
    var anchor = $(this).closest(".post-comment-wrapper").find(".post-comment-url").attr("href");
    var clone = text.clone();
    clone.find(".quote").remove();
    var quote = clone.html();
    $("html, body").animate({ scrollTop: $(document).height() }, "slow");
    tinymce.activeEditor.setContent('<div class="quote"><a class="go-to-post" data-toggle="tooltip" title="Go to post" href="' + anchor + '">' + '<span class="fas fa-arrow-alt-circle-left"></span></a>Originally posted by: ' + poster + '<br>' + quote + '</div>&#8205;<br> ', { format: 'raw' });
    tinyMCE.activeEditor.selection.select(tinyMCE.activeEditor.getBody(), true);
    tinyMCE.activeEditor.selection.collapse(false);
    tinymce.activeEditor.focus();
});

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});