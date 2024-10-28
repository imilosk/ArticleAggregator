let mix = require('laravel-mix');

mix
    .setPublicPath("./mix")
    .js('./input/js/script.js', './js')
    .version();