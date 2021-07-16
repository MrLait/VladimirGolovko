import React, {Suspense} from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import {BrowserRouter} from "react-router-dom";
import {Provider} from "react-redux";
import store from "./redux/redux-store";
import './i18n'

ReactDOM.render(
    <BrowserRouter>
        <Suspense fallback={<div>Loading...</div>}>
            <Provider store={store}>
                <App/>
            </Provider>
        </Suspense>
    </BrowserRouter>, document.getElementById('root')
);

reportWebVitals();
