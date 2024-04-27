import { useNavigate } from 'react-router-dom';
import React, { useState } from 'react';

function Home() {
    const navigate = useNavigate();
    const [loginStatus, setLoginStatus] = useState('Logged in');

    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('role');
        setLoginStatus('Logged out');
        navigate('/'); // Redirect to login page
    }

    return (
        <div>
            <h1>Welcome to the Home Page</h1>
            <p>Status: {loginStatus}</p>
            <button onClick={handleLogout}>Logout</button>
        </div>
    );
}

export default Home;
