$(() => {

    let questionId = $("#likes-count").data("question-id");

    $("#like-question").on('click', function () {
        $.post('/questions/addlike', { questionId }, function () {
            $("#like-question").addClass("text-danger");
            $("#like-question").unbind('click');
        });
    });

    setInterval(() => {
        $.get('/Questions/GetLikes', { questionId }, function (likes) {
            $("#likes-count").text(likes);
        })
    }, 500);
});