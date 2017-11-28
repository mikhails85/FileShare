const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const merge = require('webpack-merge');

const treeShakableModules = [
    '@angular/animations',
    '@angular/common',
    '@angular/compiler',
    '@angular/core',
    '@angular/forms',
    '@angular/http',
    '@angular/platform-browser',
    '@angular/platform-browser-dynamic',
    '@angular/router',
    'zone.js',
];
const nonTreeShakableModules = [
    'jquery',
    'bootstrap',
    'font-awesome/css/font-awesome.css',
    'simple-line-icons/css/simple-line-icons.css',
    'es6-promise',
    'es6-shim',
    'event-source-polyfill',
    'chart.js/dist/Chart.bundle.min.js',
    'chart.js/dist/Chart.min.js'
];
const allModules = treeShakableModules.concat(nonTreeShakableModules);

module.exports = (env) => {
    const extractCSS = new ExtractTextPlugin('vendor.css');
    const isDevBuild = !(env && env.prod);
    const sharedConfig = {
        stats: { modules: false },
        resolve: { extensions: [ '.js' ] },
        module: {
            rules: [
                { test: /\.(png|ttf|eot|woff|woff2|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/, 
                  loader: 'file-loader',
                  options: {
                      name: '[name].[ext]',
                      outputPath: 'fonts/',    // where the fonts will go
                      publicPath: '../dist/'       // override the default path
                  } 
                }
            ]
        },
        output: {
            publicPath: 'dist/',
            filename: '[name].js',
            library: '[name]_[hash]'
        },
        plugins: [
            new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery',  'window.jQuery': 'jquery',
                                    Popper: ['popper.js', 'default'] }), // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
            new webpack.ContextReplacementPlugin(/\@angular\b.*\b(bundles|linker)/, path.join(__dirname, './ClientApp')), // Workaround for https://github.com/angular/angular/issues/11580
            //new webpack.ContextReplacementPlugin(/angular(\\|\/)core(\\|\/)@angular/, path.join(__dirname, './ClientApp')), // Workaround for https://github.com/angular/angular/issues/14898
            new webpack.ContextReplacementPlugin(/angular(\\|\/)core(\\|\/)(@angular|esm5)/, path.join(__dirname, './ClientApp')),
            new webpack.IgnorePlugin(/^vertx$/) // Workaround for https://github.com/stefanpenner/es6-promise/issues/100
        ]
    };

    const styleBundleConfig = merge(sharedConfig, {
          entry: { 'style': './wwwroot/scss/style.scss' },
          output: {
            filename:  'wwwroot/dist/bundle.js',
          },
          module: {
        
            rules: [
              { // regular css files
                test: /\.css$/,
                loader: ExtractTextPlugin.extract({
                  loader: 'css-loader?importLoaders=1',
                }),
              },
              { // sass / scss loader for webpack
                test: /\.(sass|scss)$/,
                loader: ExtractTextPlugin.extract([isDevBuild ? 'css-loader' : 'css-loader?minimize', 'sass-loader'])
              }
            ]
          },
          plugins: [
            new ExtractTextPlugin({ // define where to save the file
              filename: 'wwwroot/dist/[name].bundle.css',
              allChunks: true,
            }),
          ],
        });
    
    const clientBundleConfig = merge(sharedConfig, {
        entry: {
            // To keep development builds fast, include all vendor dependencies in the vendor bundle.
            // But for production builds, leave the tree-shakable ones out so the AOT compiler can produce a smaller bundle.
            vendor: isDevBuild ? allModules : nonTreeShakableModules
        },
        output: { path: path.join(__dirname, 'wwwroot', 'dist') },
        module: {
            rules: [
                { test: /\.css(\?|$)/, use: extractCSS.extract({ use: isDevBuild ? 'css-loader' : 'css-loader?minimize' }) },
                { test: /\.scss$/, use: [{ loader: isDevBuild ? 'css-loader' : 'css-loader?minimize'}, {loader: "sass-loader" }] }
            ]
        },
        plugins: [
            extractCSS,
            new webpack.DllPlugin({
                path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
                name: '[name]_[hash]'
            })
        ].concat(isDevBuild ? [] : [
            new webpack.optimize.UglifyJsPlugin()
        ])
    });
    const serverBundleConfig = merge(sharedConfig, {
        target: 'node',
        resolve: { mainFields: ['main'] },
        entry: { vendor: allModules.concat(['aspnet-prerendering']) },
        output: {
            path: path.join(__dirname, 'ClientApp', 'dist'),
            libraryTarget: 'commonjs2',
        },
        module: {
            rules: [ { test: /\.css(\?|$)/, use: ['to-string-loader', isDevBuild ? 'css-loader' : 'css-loader?minimize' ] },
                     { test: /\.scss$/, use: [{loader: 'to-string-loader'}, { loader: "css-loader"}, {loader: "sass-loader" }] }
            ]
        },
        plugins: [
            new webpack.DllPlugin({
                path: path.join(__dirname, 'ClientApp', 'dist', '[name]-manifest.json'),
                name: '[name]_[hash]'
            })
        ]
    });

    return [clientBundleConfig, serverBundleConfig, styleBundleConfig];
}
