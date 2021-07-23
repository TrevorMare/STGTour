﻿## Using NPM Packages

To install a new package, cli to the npm directory and run the npm install module command.

After installation, add the "dist" output of the package to the bundleconfig.json file 

To add CSS Files

[
  {
    "outputFileName": "wwwroot/css/site.min.css",
    // An array of relative input file paths. Globbing patterns supported
    "inputFiles": [
      "wwwroot/css/site.css"
    ],
    "minify": { "enabled": true }
  },
  {
    "outputFileName": "wwwroot/js/site.min.js",
    "inputFiles": [
      "wwwroot/js/site.js"
    ],
    // Optionally specify minification options
    "minify": {
      "enabled": true,
      "renameLocals": true
    },
    // Optinally generate .map file
    "sourceMap": false
  },
  // Vendor CSS
  {
    "outputFileName": "wwwroot/css/vendor.min.css",
    "inputFiles": [
      "node_modules/bootstrap/dist/css/bootstrap.min.css"
    ],
    "minify": { "enabled": true }
  },
  // Vendor JS
  {
    "outputFileName": "wwwroot/js/vendor.min.js",
    "inputFiles": [
      "node_modules/jquery/dist/jquery.min.js",
      "node_modules/bootstrap/dist/js/bootstrap.bundle.min.js"
    ],
    "minify": {
      "enabled": true,
      "renameLocals": true
    },
    "sourceMap": false
  },
  // Vendor Validation JS
  {
    "outputFileName": "wwwroot/js/vendor-validation.min.js",
    "inputFiles": [
      "node_modules/jquery-validation/dist/jquery.validate.min.js",
      "node_modules/jquery-validation/dist/additional-methods.min.js",
      "node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js",
      "node_modules/jquery-ajax-unobtrusive/dist/jquery.unobtrusive-ajax.min.js"
    ],
    "minify": {
      "enabled": true,
      "renameLocals": true
    },
    "sourceMap": false
  }
]