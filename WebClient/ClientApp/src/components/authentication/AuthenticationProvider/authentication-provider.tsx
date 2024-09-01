import { useEffect } from "react";

import { ApiClient } from "../../api/api-client";
import { Url } from '../../constants/url';
import { matchPath, useNavigate } from 'react-router-dom';
import { authenticate, deauthenticate } from '../../../redux/authentication-slice';
import { useDispatch } from 'react-redux';


export function AuthenticationProvider() {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  useEffect(() => {
    ApiClient.initialize().then((result) => result ? authenticateUser() : redirectToLogin());
  }, []);
  
  function authenticateUser() {
    dispatch(authenticate());
  }
  
  function redirectToLogin() {
    dispatch(deauthenticate());
    const location = window.location.pathname;
    const isViableRoute = matchPath(location, Url.Authentication.Login);
    if (!isViableRoute) {
      navigate(Url.Authentication.Login);
    }
  }
  
  return null;
}