/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './*.cshtml',
        '../output/**/*.{html,js}'
    ],
    darkMode: ["class", '[data-theme="dark"]'],
    theme: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/typography')
    ],
}

