/// <reference path="jquery-1.4.1.js" />
/// <reference path="Routing.js" />

$(document).ready(function () {
    alert($.routeManager.controllers.Home.Index().toUrl());
});