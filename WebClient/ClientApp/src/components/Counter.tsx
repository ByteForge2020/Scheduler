import React, { useState } from 'react';
import { useDispatch, useSelector } from "react-redux";
import { authenticate, deauthenticate } from "../redux/authentication-slice";
import { RootState } from "../redux/store";

export default function Counter() {
    const [currentCount, setCurrentCount] = useState<number>(0);
    const dispatch = useDispatch();
    const isAuthenticated = useSelector((state: RootState) => state.authentication.value);

    const incrementCounter = () => {
        if (currentCount % 2 === 0) {
            dispatch(authenticate());
        } else {
            dispatch(deauthenticate());
        }
        setCurrentCount(currentCount + 1);
    };

    const check = () => {
        console.log(isAuthenticated);
    };

    return (
        <div>
            <h1>Counter</h1>

            <p>This is a simple example of a React component.</p>

            <p aria-live="polite">Current count: <strong>{currentCount}</strong></p>
            <button className="btn btn-primary" onClick={check}>Check</button>
            <button className="btn btn-primary" onClick={incrementCounter}>Increment</button>
        </div>
    );
};