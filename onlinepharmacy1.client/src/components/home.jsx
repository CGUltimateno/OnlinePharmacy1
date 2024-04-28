import { useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import CryptoJS from 'crypto-js';

function Home() {
    const navigate = useNavigate();
    const [loginStatus, setLoginStatus] = useState('Logged in');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');

    const token = localStorage.getItem('token');
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    const decodedToken = JSON.parse(jsonPayload);
    const userId = decodedToken.sub;

    useEffect(() => {
        fetch(`http://localhost:5209/api/Account/GetUserDetails?userId=${userId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                setFirstName(data.firstName);
                setLastName(data.lastName);
            })
            .catch(error => {
                console.log('Fetch error: ', error);
            });
    }, [userId]);

    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('role');
        setLoginStatus('Logged out');
        navigate('/'); // Redirect to login page
    }

    return (
        <div>
            <h1>Welcome to the Home Page</h1>
            <p>Full Name: {firstName} {lastName}</p>
            <p>Status: {loginStatus}</p>
            <p>Click the button below to logout</p>
            <button onClick={handleLogout}>Logout</button>
        </div>
    );
}

export default Home;
