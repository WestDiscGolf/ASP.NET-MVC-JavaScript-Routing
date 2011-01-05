module("Array");

test("Array.any return false when array is empty", function () {
    ok(!([].any(function () { return true; })));
});

test("Array.any return false when array does not contain value", function () {
    ok(!(["foo", "bar"].any(function (str) { return str.length > 3; })));
});

test("Array.any return true when array contains value satisfying predicate", function () {
    ok(["foo", "bar"].any(function (str) { return str.length == 3; }));
});

test("Array.any return true when array contains value equal to the parameter of the method when not a function", function () {
    ok(["foo", "bar"].any("foo"));
});

test("Array.any return false when array does not contains value equal to the parameter of the method when not a function", function () {
    ok(!["foo", "bar"].any("baz"));
});

module("Route Manager", {
    setup: function () {
        $.routeManager.routes = [];
        $.routeManager.namedRoutes = {};
        $.mockjaxClear();
    }
});

test("defined on jquery object", function () {
    ok($.routeManager !== undefined);
    ok($.routeManager.controllers !== undefined);
    same($.routeManager.constraintTypeDefs, {});
});

test("formatUrl works with empty data", function () {
    equal($.routeManager.formatUrl("/foo/data"), "/foo/data");
});

test("formatUrl works with data values", function () {
    equal($.routeManager.formatUrl("/foo/{controller}", { controller: "blah", action: "fi" }), "/foo/blah");
});

test("formatUrl adds slash in from when not present in pattern", function () {
    equal($.routeManager.formatUrl("foo/{controller}", { controller: "blah", action: "fi" }), "/foo/blah");
});

test("formatUrl removes default parameters at the end", function () {
    equal($.routeManager.formatUrl("foo/{controller}/{action}", { controller: 'blah', action: 'index' }, { controller: "yum", action: 'index' }),
          "/foo/blah");
});

test("formatUrl removes all default parameters at the end", function () {
    equal($.routeManager.formatUrl("foo/{controller}/{action}", { controller: 'blah', action: 'index' }, { controller: "blah", action: 'index' }),
          "/foo");
});

test("formatUrl does not remove default parameters not at the end", function () {
    equal($.routeManager.formatUrl("foo/{action}/{controller}", { controller: 'blah', action: 'index' }, { controller: "yum", action: 'index' }),
          "/foo/index/blah");
});

test("mapRoute adds route to route table", function () {
    $.routeManager.mapRoute("/foo/bar");
    equal($.routeManager.routes.length, 1);
});

test("mapRoute adds route to route table with pattern specified", function () {
    var pattern = "/foo/bar/baz";
    $.routeManager.mapRoute(pattern);
    equal($.routeManager.routes[0].pattern, pattern);
});

test("mapRoute adds route to route table with route name specified", function () {
    var name = "route name";
    $.routeManager.mapRoute("/foo", name);
    equal($.routeManager.routes[0].name, name);
});

test("mapRoute adds route to route table with empty route name when not specified", function () {
    $.routeManager.mapRoute("/foo");
    equal($.routeManager.routes[0].name, "");
});

test("mapRoute returns route from method", function () {
    equal($.routeManager.mapRoute("/foo").pattern, "/foo");
});

test("mapRoute adds default values to the route definition", function () {
    var defaultValues = { name: "foo", m: 5 };
    equal($.routeManager.mapRoute("/foo", '', defaultValues).defaultValues, defaultValues);
});

test("mapRoute adds empty default values to the route definition when none specified", function () {
    same($.routeManager.mapRoute("/foo", '').defaultValues, {});
});

test("route accept returns false when data does not contain controller", function () {
    ok(!$.routeManager.mapRoute('/foo').accept({}));
});

test("route accept returns false when data does not contain action", function () {
    ok(!$.routeManager.mapRoute('/foo').accept({controller:'foo'}));
});

test("route accept returns false when route does not contain controller paramter and default value's controller is not same", function () {
    var route = $.routeManager.mapRoute('/foo/', '', { controller: 'foo', action: 'bar' });
    ok(!route.accept({ controller: "baz", action: "boo" }));
});

test("route accept returns true when route contains same controller name in routing data passed to accept", function () {
    var route = $.routeManager.mapRoute('/foo', '', { controller: 'foo', action: 'bar' });
    ok(route.accept({ controller: "foo", action: "bar" }));
});

test("route parameters returns value with controllers as parameter when controller appears as a parameter", function () {
    var route = $.routeManager.mapRoute('{controller}/{action}', '', { action: 'bar' });
    same(route.params[0], "controller");
});

test("route accept returns true when route contains controller in parameter and matching action", function () {
    var route = $.routeManager.mapRoute('{controller}/a', '', { action: 'bar' });
    ok(route.accept({ controller: "foo", action:"bar" }));
});

test("route accept returns false when action not a parameter and not equal default", function () {
    var route = $.routeManager.mapRoute('{controller}/a', '', { action: 'bar' });
    ok(!route.accept({ controller: 'foo', action: 'baz' }));
});

test("route accept returns true when action not a parameter and equal default", function () {
    var route = $.routeManager.mapRoute('{controller}/a', '', { action: 'bar' });
    ok(route.accept({ controller: 'foo', action: 'bar' }));
});

test("route accept returns false when a constraint returns false", function () {
    var route = $.routeManager.mapRoute('{controller}/{action}', '', { action: 'bar' });
    route.constraints.push(function(data){ return false; });
    ok(!route.accept({ controller: 'foo', action: 'bar' }));
});

test("route accept called with data passed to the route accept method", function () {
    var route = $.routeManager.mapRoute('{controller}/{action}', '', { action: 'bar' });
    var val = { controller: 'foo', action: 'bar' };
    
    route.constraints.push(function (data) { same(data, val); return false; });

    ok(!route.accept(val));
});

test("route accept returns false when params of url contains area keyword and the route data does not contain area", function () {
    var route = $.routeManager.mapRoute('{area}/{controller}/{action}', '', { action: 'bar' });
    var val = { controller: 'foo', action: 'bar' };
    ok(!route.accept(val));
});

test("action returns true when route is accepted", function () {
    $.routeManager.mapRoute('foo/bar', '', { controller: 'foo', action: 'bar' });
    ok($.routeManager.action({ controller: "foo", action: "bar" }));
});

test("action returns result with formatted URL in toUrl", function () {
    $.routeManager.mapRoute('{controller}/{action}', '', {});
    equal($.routeManager.action({ controller: "foo", action: "bar" }).toUrl(), "/foo/bar");
});

test("action returns result with formatted URL in toUrl contains default values when not specified by data", function () {
    $.routeManager.mapRoute('{id}/{controller}/{action}', '', { id: "4" });
    equal($.routeManager.action({ controller: "foo", action: "bar" }).toUrl(), "/4/foo/bar");
});

test("action returns result that can perform post", function () {
    expect(4);
    $.routeManager.mapRoute('{controller}/{action}', '');
    
    var extraData = { extra: 'data' };
    var callback = function (foo) { return ""; };
    var dataType = "application/javascript";

    $.mockjax({
        url: '/foo/bar',
        response: function (data) {
            same(data.data, extraData);
            equal(data.type, "POST");
            equal(data.success, callback);
            equal(data.dataType, dataType);
            start();
            return "";
        }
    });

    stop();
    $.routeManager.action({ controller: "foo", action: "bar" }).post(extraData, callback, dataType);
});

test("action returns result that can perform get", function () {
    expect(4);
    $.routeManager.mapRoute('{controller}/{action}', '');

    var extraData = { extra: 'data' };
    var callback = function (foo) { return ""; };
    var dataType = "application/javascript";

    $.mockjax({
        url: '/foo/bar',
        response: function (data) {
            same(data.data, extraData);
            equal(data.type, "GET");
            equal(data.success, callback);
            equal(data.dataType, dataType);
            start();
            return "";
        }
    });

    stop();
    $.routeManager.action({ controller: "foo", action: "bar" }).get(extraData, callback, dataType);
});

test("action returns result with get that adds unused default values in the extra params of get request", function () {
    expect(1);
    $.routeManager.mapRoute('{controller}/{action}', '', { id: 5 });

    $.mockjax({
        url: '/foo/bar',
        response: function (data) {
            same(data.data, { id: 5 });
            start();
            return "";
        }
    });

    stop();
    $.routeManager.action({ controller: "foo", action: "bar" }).get();
});

test("action returns result with get that adds unused route in the extra params of get request", function () {
    expect(1);
    $.routeManager.mapRoute('{controller}/{action}', '', {});

    $.mockjax({
        url: '/foo/bar',
        response: function (data) {
            same(data.data, { id: 5 });
            start();
            return "";
        }
    });

    stop();
    $.routeManager.action({ controller: "foo", action: "bar", id: 5 }).get();
});

test("action returns result with get that adds unused default values in the extra params of post request", function () {
    expect(1);
    $.routeManager.mapRoute('{controller}/{action}', '', { id: 5 });

    $.mockjax({
        url: '/foo/bar',
        response: function (data) {
            same(data.data, { id: 5 });
            start();
            return "";
        }
    });

    stop();
    $.routeManager.action({ controller: "foo", action: "bar" }).get();
});

test("action returns result with post that adds unused route in the extra params of post request", function () {
    expect(1);
    $.routeManager.mapRoute('{controller}/{action}', '', {});

    $.mockjax({
        url: '/foo/bar',
        response: function (data) {
            equal(data.data.id, 5);
            start();
            return "";
        }
    });

    stop();
    $.routeManager.action({ controller: "foo", action: "bar", id: 5 }).post();
});

test("mapRoute adds named route to namedRoute property when name defined", function () {
    var defaultValues = { name: "foo", m: 5 };
    var name = "namedRoute";
    var route = $.routeManager.mapRoute("/foo", name, defaultValues);
    same($.routeManager.namedRoutes[name], route);
});

test("namedAction returns undefined when named route not present", function () {
    same($.routeManager.namedAction("name"), undefined);
});

test("namedAction returns route when named route present", function () {
    var name = "namedRoute";
    var route = $.routeManager.mapRoute("/foo", name, { name: "foo", m: 5 });
    route.accept = function () { return true; };
    same($.routeManager.namedAction(name, {}).toUrl(), "/foo");
});

test("namedAction returns undefined when named route does not accept data", function () {
    var name = "namedRoute";
    var route = $.routeManager.mapRoute("/foo", name, { name: "foo", m: 5 });
    route.accept = function () { return false; };
    same($.routeManager.namedAction(name, {}), undefined);
});

test("mapControllerAction adds controller when undefined", function () {
    $.routeManager.controllers = {};
    var controller = 'myController';
    $.routeManager.mapControllerAction(controller, 'action');
    ok($.routeManager.controllers[controller]);
});

test("mapControllerAction adds action to controllers that returns function", function () {
    $.routeManager.mapRoute("{controller}/{action}/{id}", '', {});
    var controller = 'myController';
    var action = 'action';
    var fn = function () { return false };
    $.routeManager.mapControllerAction(controller, action, fn);
    same(fn, $.routeManager.controllers[controller][action]);
});

test("mapControllerAction adds action with different name when action already exists", function () {
    $.routeManager.mapRoute("{controller}/{action}/{id}", '', {});
    var controller = 'myController';
    var action = 'action';
    var fn = "fn";

    $.routeManager.controllers = {};

    $.routeManager.mapControllerAction(controller, action, fn);

    var fn2 = "fn2"
    $.routeManager.mapControllerAction(controller, action, fn2);
    same(fn, $.routeManager.controllers[controller][action]);
    same(fn2, $.routeManager.controllers[controller][action + "1"]);
});

test("action returns result with get that is chainable", function () {
    expect(1);
    $.routeManager.mapRoute('{controller}/{action}', '', { id: 5 });

    $.mockjax({
        url: '/foo/bar',
        response: function (data) {
            start();
            return "";
        }
    });

    stop();
    equal($.routeManager.action({ controller: "foo", action: "bar" }).get().toUrl(), "/foo/bar");
});

test("action returns result with post that is chainable", function () {
    expect(1);
    $.routeManager.mapRoute('{controller}/{action}', '', { id: 5 });

    $.mockjax({
        url: '/foo/bar',
        response: function (data) {
            start();
            return "";
        }
    });

    stop();
    equal($.routeManager.action({ controller: "foo", action: "bar" }).post().toUrl(), "/foo/bar");
});