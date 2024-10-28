/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './input/*.cshtml',
        './output/**/*.{html,js}'
    ],
    darkMode: ["class", '[data-theme="dark"]'],
    theme: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/typography')
    ],
}

