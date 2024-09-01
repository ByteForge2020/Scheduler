import React, { useEffect } from 'react';
import { Route, Routes } from 'react-router-dom';
import { Layout } from './components/Layout';
import './custom.css';
import { useAppSelector } from "./redux/hooks";
import { Url } from "./components/constants/url";
import Counter from './components/Counter';
import SignIn from './components/authentication/Login';
import SchedulerAppointments from './components/SchedulerAppointments';
import { selectAuthenticationState } from './redux/store';

const App = () => {
    const dotenv = require("dotenv");
    dotenv.config();
    const isAuthenticated = useAppSelector(selectAuthenticationState);
    return (
        <Layout>
            <Routes>
                {isAuthenticated ? (
                    <>
                        <Route path={Url.Default} element={<Counter />} />
                        <Route path={Url.Appointments.Main} element={<SchedulerAppointments />} />
                        <Route path={Url.Authentication.Login} element={<SignIn />} />
                    </>
                ) : (
                    <Route path={Url.Authentication.Login} element={<SignIn />} />
                )}
            </Routes>
                
        </Layout>
    );
};

export default App;