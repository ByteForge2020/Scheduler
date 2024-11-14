import React, { useEffect } from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';

import './custom.css';
import { useAppSelector } from './redux/hooks';
import { Url } from './components/constants/url';
import SignIn from './components/authentication/Login';

import { selectAuthenticationState } from './redux/store';
import SchedulerAppointments from './components/Scheduler/schedulerAppointments';
import { Layout } from './components/Layout';
import CustomerDataGrid from './components/Customers/customersDataGrid';

const App = () => {
    const isAuthenticated = useAppSelector(selectAuthenticationState);
    
    return (
        <Layout>
            <Routes>
                {isAuthenticated ? (
                    <>
                        <Route path={Url.Default} element={<SchedulerAppointments />} />
                        <Route path={Url.Appointments.Main} element={<SchedulerAppointments />} />
                        <Route path={Url.Customers.Main} element={<CustomerDataGrid />} />
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