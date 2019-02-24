const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = (env = {}, argv = {}) => {
  const isProd = argv.mode === "production";

  const config = {
    mode: argv.mode || "development",

    entry: {
      app: "./js/app.ts",
      spa: "./js/spa.tsx"
    },

    output: {
      filename: "[name].bundle.js",
      path: path.resolve(__dirname, "../wwwroot/dist"),
      publicPath: "/dist/"
    },

    resolve: {
      // Add `.ts` and `.tsx` as a resolvable extension.
      extensions: [".ts", ".tsx", ".js"]
    },

    plugins: [
      new MiniCssExtractPlugin({
        filename: "styles.css"
      })
    ],

    module: {
      rules: [
        {
          test: /\.css$/,
          use: [
            isProd ? MiniCssExtractPlugin.loader : "style-loader",
            "css-loader"
          ]
        },
        {
          test: /\.tsx?$/,
          use: "awesome-typescript-loader",
          exclude: /node_modules/
        }
      ]
    }
  };

  if (!isProd) {
    config.devtool = 'eval-source-map';
  }

  return config;
};
