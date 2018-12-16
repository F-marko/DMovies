function getMovie(id) {
    $.ajax({
        url: 'api/movie',
        method: 'GET',
        headers: {
            'content-type': 'application/json'
        },
        data: {
            imdbId: id
        },
        success: function (response) {
            window.location.href = "Details.html";

            $('#movieInfo').empty();

            var actors = '';
            response.Actors.split(',').forEach(function (actor) {
                actors += '<a onclick="getActor(\'' + actor + '\')" href="#">' + actor + ',</a>';
            });

            var info =
                '<div class="row">'
                + '<div class="col-md-4">'
                + '<img src="' + response.Poster + '" class="thumbnail">'
                + '</div>'
                + '<div class="col-md-8">'
                + '<h2>' + response.Title + '</h2>'
                + '<ul class="list-group">'
                + '<li class="list-group-item"><strong>Genre:</strong> ' + response.Genre + '</li>'
                + '<li class="list-group-item"><strong>Released:</strong> ' + response.Released + '</li>'
                + '<li class="list-group-item"><strong>Rated:</strong> ' + response.Rated + '</li>'
                + '<li class="list-group-item"><strong>IMDB Rating:</strong> ' + response.imdbRating + '</li>'
                + '<li class="list-group-item"><strong>Director:</strong> ' + response.Director + '</li>'
                + '<li class="list-group-item"><strong>Actors:</strong> ' + actors + '</li>'
                + '</div>'
                + '<div class="row">'
                + '<div class="well">'
                + '<h3>Plot</h3>'
                + response.Plot
                + '<hr>'
                + '<a href="http://imdb.com/title/' + response.imdbID + '" target="_blank" class="btn btn-primary">Go to IMDB</a>'
                + '</div>'
                + '</div>';

            $('#movieInfo').append(info);
        }
    });
}

function getActor(actor) {
    $.ajax({
        url: 'api/Movies/GetActor',
        method: 'GET',
        headers: {
            'content-type': 'application/json'
        },
        data: {
            name: actor
        },
        success: function (response) {
            $('#actorInfo').removeClass('hidden');

            $('#actorInfo').empty();

            var info =
                '<div class="row">'
                + '<div class="col-md-4">'
                + '<img src="' + response.image.medium + '" class="thumbnail">'
                + '</div>'
                + '<div class="col-md-8">'
                + '<h2>' + response.name + '</h2>'
                + '<ul class="list-group">'
                + '<li class="list-group-item"><strong>Birthday:</strong> ' + response.birthday + '</li>'
                + '<li class="list-group-item"><strong>Nationality:</strong> ' + response.country.name + '</li>'
                + '<li class="list-group-item"><strong>Gender:</strong> ' + response.gender + '</li>'
                + '</div>';

            $('#actorInfo').append(info);
        }
    });
}

function addToWatchlist(id) {
    var user = sessionStorage.getItem('userName');
    $.ajax({
        url: 'api/Movies/Add',
        method: 'GET',
        headers: {
            'content-type': 'application/json'
        },
        data: {
            user: user,
            imdbId: id
        },
        success: function () {
            alert('Movie added to watchlist!');
        }
    });
}