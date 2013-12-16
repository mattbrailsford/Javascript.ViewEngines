JsViewEngines.add({
    name: "Resig",
    extensions: ["resig"],
    template: new Template(function (template, model) {
        return tmpl(template, model);
    }),
    requires: ["resig.js"]
});

JsViewEngines.add({
    name: "Haml-js",
    extensions: ["haml"],
    template: new Template(function (template, model) {
        return Haml.render(template, { context: model });
    }),
    requires: ["haml.js"]
});

JsViewEngines.add({
    name: "Handlebars",
    extensions: ["hb", "hbs", "handlebars"],
    template: new Template(function (template, model) {
        return Handlebars.compile(template)({ model: model });
    }),
    requires: ["handlebars.js"]
});

JsViewEngines.add({
    name: "Jade",
    extensions: ["jade"],
    template: new Template(function (template, model) {
        return jade.compile(template)(model);
    }),
    requires: ["jade.js"]
});

JsViewEngines.add({
    name: "Mustache",
    extensions: ["mustache"],
    template: new Template(function (template, model) {
        return Mustache.render(template, model);
    }),
    requires: ["mustache.js"]
});

JsViewEngines.add({
    name: "_",
    extensions: ["us"],
    template: new Template(function (template, model) {
        return _.template(template, model);
    }),
    requires: ["underscore.js"]
});

JsViewEngines.add({
    name: "Vash",
    extensions: ["vash"],
    template: new Template(function (template, model) {
        return vash.compile(template)(model);
    }),
    requires: ["vash.js"]
});