import { useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import './Home.css';
function Home() {
  const navigate = useNavigate();
  const [loginStatus, setLoginStatus] = useState('Logged in');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
    const [userId, setUserId] = useState('');
  const [userData, setUserData] = useState({}); // Use a single state object

  useEffect(() => {
    const token = localStorage.getItem('token'); // Assuming token exists

    if (!token) {
      // Handle no token case (e.g., redirect to login)
      return;
    }

    fetch(`http://localhost:5209/api/Account/GetUserDetails`, {
      headers: {
        Authorization: `Bearer ${token}`, // Include JWT in Authorization header
      },
    })
      .then(response => {
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
      })
      .then(data => {
        setUserData(data); // Set complete user data object
      })
      .catch(error => {
        console.error('Fetch error: ', error);
        // Handle potential errors (e.g., display error message)
      });
  }, []); // Empty dependency array to fetch data once on component mount

  const handleLogout = () => {
    localStorage.removeItem('token');
    setLoginStatus('Logged out');
    navigate('/'); // Redirect to login page
  };

  return (
      <div className="home-container" >
      <h1>Welcome to the Home Page</h1>
      {userData && ( // Check if userData exists
              <div className="user-info-container">
                  <p>First Name: {userData.firstName}</p>
                  <p>Last Name: {userData.lastName}</p>
                  <p>User ID: {userData.userId}</p>
              </div>
      )}
      <p>Status: {loginStatus}</p>
      <p>Click the button below to logout</p>
      <button onClick={handleLogout}>Logout</button>
    </div>
  );
}

export default Home;
