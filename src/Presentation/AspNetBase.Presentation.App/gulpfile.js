const { src, dest } = require("gulp");
const concat = require("gulp-concat");

/**
 * Copies selected directories from `~/node_modules` into `~/wwwroot/lib`.
 * @param {any} cb Gulp required callback invoked at the end of the task.
 */
function buildLibBundle(cb) {
  src([
    "node_modules/jquery/dist/jquery.slim.min.js",
    "node_modules/jquery-validation/dist/jquery.validate.min.js",
    "node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js",
    "node_modules/popper.js/dist/popper.min.js",
    "node_modules/bootstrap/dist/js/bootstrap.min.js",
    "wwwroot/vendor/mvcgrid/mvc-grid.min.js"
  ])
    .pipe(concat("lib.bundle.min.js"))
    .pipe(dest("wwwroot/lib"));

  src([
    "node_modules/bootstrap/dist/css/bootstrap.min.css",
    "wwwroot/vendor/mvcgrid/mvc-grid.min.css"
  ]).pipe(concat("lib.bundle.min.css")).pipe(dest("wwwroot/lib"));

  src("wwwroot/vendor/mvcgrid/GridGlyphs.woff").pipe(dest("wwwroot/lib"));

  cb();
}

/**
 * Copies selected directories from ~/node_modules into ~/wwwroot/lib/node_modules.
 * @param {any} cb Gulp required callback invoked at the end of the task.
 */
function buildAppBundle(cb) {}

exports.default = buildLibBundle;
exports.buildLibBundle = buildLibBundle;
