// ! requirejs config strategies:
// https://github.com/requirejs/requirejs/wiki/Patterns-for-separating-config-from-the-main-module

// * NOTE: this is pre requirejs load config strategy and this
// * script has to be loaded before loading requirejs script
var require = {
  // * NOTE: due to module resolution is needed for only
  // * typescript source compile into javascript
  baseUrl: "../dist/app/js",
  paths: {
    // NOTE: due to tsconfig.json --importHelpers, relative to baseUrl
    tslib: "../../lib/tslib.min"
  },
  waitSeconds: 15
};
