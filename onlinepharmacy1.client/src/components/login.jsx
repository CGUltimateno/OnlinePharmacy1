import { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import jwtDecode from 'jwt-decode'; // Import decode function directly

function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loginStatus, setLoginStatus] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (event) => {
        event.preventDefault();

        const formData = new FormData();
        formData.append('email', email);
        formData.append('password', password);

        try {
            const response = await axios.post("http://localhost:5209/api/Account/Login", formData);
        
            if (response.status === 200) {
                localStorage.setItem('token', response.data.token);
                localStorage.setItem('userId', response.data.userId);
                setLoginStatus('Login successful');
        
                // Decode the token and check the user's role
                const decodedToken = jwtDecode(response.data.token); // Use jwtDecode to decode the token
                const userRole = decodedToken.role;
        
                // Redirect the user based on their role
                if (userRole === 'Admin') {
                    navigate('/admin');
                } else {
                    navigate('/home');
                }
            } else {
                throw new Error('Login failed');
            } 
        } catch (error) {
            setLoginStatus('Login failed');
            console.error('Error:', error);
        }
    }; 

    return (
        <div>
            <h1>Login</h1>
            <form onSubmit={handleLogin}>
                <label>
                    Email:
                    <input type="text" value={email} onChange={e => setEmail(e.target.value)} />
                </label>
                <label>
                    Password:
                    <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
                </label>
                <input type="submit" value="Submit" />
            </form>
            <button onClick={() => window.location.href = '/register'}>Register</button>
            <button onClick={() => window.location.href = '/registerAdmin'}>Register for Admin</button>
            <p>{loginStatus}</p>
        </div>
    );
}

export default Login;
