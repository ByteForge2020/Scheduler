import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import * as serviceWorkerRegistration from './serviceWorkerRegistration';
import { Provider } from 'react-redux';
import store from './redux/store';
import {
    AuthenticationProvider
} from './components/authentication/AuthenticationProvider/authentication-provider';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');
let root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement);

if (process.env.NODE_ENV !== "production") {
    import((`hide-cra-error-overlay`)).then(({ initHideOverlay }) =>
        initHideOverlay()
    );
}


root.render(
    <Provider store={store}>
        <BrowserRouter basename={baseUrl || '/'}>
            
            <AuthenticationProvider />
            <App/>
        
        
        </BrowserRouter>
    </Provider>
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.unregister();