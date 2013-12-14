engines.add({
    name: "Resig",
    extensions: ["resig"],
    template: new Template(function (template, model) {
        return tmpl(template, model);
    }),
    requires: ["resig.js"]
});

engines.add({
    name: "Haml-js",
    extensions: ["haml"],
    template: new Template(function (template, model) {
        return Haml.render(template, { context: model });
    }),
    requires: ["haml.js"]
});

engines.add({
    name: "Handlebars",
    extensions: ["hb", "hbs", "handlebars"],
    template: new Template(function (template, model) {
        return Handlebars.compile(template)({ model: model });
    }),
    requires: ["handlebars.js"]
});

engines.add({
    name: "Jade",
    extensions: ["jade"],
    template: new Template(function (template, model) {
        return jade.compile(template)(model);
    }),
    requires: ["jade.js"]
});

    engines.add({
        name: "Mustache",
        extensions: ["mustache"],
        template: new Template(function (template, model) {
            return Mustache.render(template, model);
        }),
        requires: ["mustache.js"]
    });

engines.add({
    name: "_",
    extensions: ["us"],
    template: new Template(function (template, model) {
        return _.template(template, model);
    }),
    requires: ["underscore.js"]
});

engines.add({
    name: "Vash",
    extensions: ["vash"],
    template: new Template(function (template, model) {
        return vash.compile(template)(model);
    }),
    requires: ["vash.js"]
});