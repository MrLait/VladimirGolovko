import React from "react";
import s from './Header.module.css';
import {NavLink} from "react-router-dom";

const Header = (props) => {
    return (
        <header className={s.header}>

            <div className={s.homeBlock}>
                <NavLink to={'/home'}>Home</NavLink>
            </div>

            <div className={s.loginBlock}>
                {props.isAuth ? props.token
                    : <NavLink to={'/login'}>Login</NavLink>}
            </div>

        </header>
    );
}

export default Header;