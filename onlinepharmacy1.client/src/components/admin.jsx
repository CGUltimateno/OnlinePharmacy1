import React, { useState } from 'react';
import { Link } from 'react-router-dom';
 // Assuming you have an AuthContext for user authentication
import './AdminNavbar.css';


function AdminNavbar() {
  const [activeTab, setActiveTab] = useState('list-patients');
 // Get user authentication and admin status

  const handleTabClick = (tab) => {
    setActiveTab(tab);
  };

  return (
    // Only render navbar if user is logged in and is admin
      <nav className="admin-navbar">
        <ul>
          <li
            className={activeTab === 'list-patients' ? 'active' : ''}
            onClick={() => handleTabClick('list-patients')}
          >
            <Link to="/admin/list-patients">List Patients</Link>
          </li>
          <li
            className={activeTab === 'add-patient' ? 'active' : ''}
            onClick={() => handleTabClick('add-patient')}
          >
            <Link to="/admin/add-patient">Add Patient</Link>
          </li>
          <li
            className={activeTab === 'update-patient' ? 'active' : ''}
            onClick={() => handleTabClick('update-patient')}
          >
            <Link to="/admin/update-patient">Update Patient</Link>
          </li>
          <li
            className={activeTab === 'delete-patient' ? 'active' : ''}
            onClick={() => handleTabClick('delete-patient')}
          >
           <Link to="/admin/delete-patient">Delete Patient</Link>
          </li>
        </ul>
      </nav>
    )
  
}

export default AdminNavbar;