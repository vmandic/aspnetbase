const { src, dest, series, parallel } = require("gulp");
const concat = require("gulp-concat");
const uglify = require("gulp-uglify");
const ren = require("gulp-rename");
const rimraf = require("rimraf");

/**
 * Copies selected directories from `~/Client/node_modules` into `~/wwwroot/lib`.
 * @param {any} cb Gulp required callback invoked at the end of the task.
 */
function addLibBundle(cb) {
  src([
    "node_modules/jquery/dist/jquery.slim.min.js",
    "node_modules/jquery-validation/dist/jquery.validate.min.js",
    "node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js",
    "node_modules/bootstrap/dist/js/bootstrap.bundle.min.js",
    "vendor/mvcgrid/mvc-grid.min.js"
  ])
    .pipe(concat("lib.bundle.min.js"))
    .pipe(dest("../wwwroot/dist/lib"));

  src([
    "node_modules/bootstrap/dist/css/bootstrap.min.css",
    "vendor/mvcgrid/mvc-grid.min.css"
  ])
    .pipe(concat("lib.bundle.min.css"))
    .pipe(dest("../wwwroot/dist/lib"));

  src("vendor/mvcgrid/GridGlyphs.woff").pipe(dest("../wwwroot/dist/lib"));

  cb();
}

function clearDistLib(cb) {
  rimraf("../wwwroot/dist/lib", () => cb());
}

function clearDistApp(cb) {
  rimraf("../wwwroot/dist/app", () => cb());
}

function configureAndAddRequirejs(cb) {
  src(["require.config.js", "node_modules/requirejs/require.js"])
    .pipe(concat("require.min.js"))
    .pipe(uglify())
    .pipe(dest("../wwwroot/dist/lib"));
  cb();
}

function addTsLib(cb) {
  src("node_modules/tslib/tslib.js")
    .pipe(
      uglify({
        mangle: false,
        keep_fnames: true
      })
    )
    .pipe(ren("tslib.min.js"))
    .pipe(dest("../wwwroot/dist/lib"));

  cb();
}

function addIcons(cb) {
  src("icons/**/*").pipe(dest("../wwwroot/icons"));
  cb();
}

exports.default = series(
  clearDistLib,
  parallel(addLibBundle, addTsLib, configureAndAddRequirejs, addIcons)
);

exports.build = series(
  parallel(clearDistLib, clearDistApp),
  parallel(addLibBundle, addTsLib, configureAndAddRequirejs, addIcons)
);
