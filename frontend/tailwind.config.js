export default {
  content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    extend: {
      fontFamily: {
        inter: ['"Inter"', 'sans-serif'],
        pixel: ['"Press Start 2P"', 'monospace'],
      },
    },
  },
  plugins: [require('tailwindcss-animate')],
}
