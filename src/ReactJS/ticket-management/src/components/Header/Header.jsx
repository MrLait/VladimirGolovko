import React from "react";
import s from './Header.module.css';
import {NavLink} from "react-router-dom";
import {logout} from "../../redux/auth-reducer";

const Header = (props) => {
    return (
        <header className={s.header}>
            <div className={s.homeBlock}>
                <NavLink to={'/home'}>Home</NavLink>
            </div>

            <div className={s.loginBlock}>
                {
                    props.isAuth ?
                        <div>
                            <span className={s.span}>{props.email}</span>
                            <span className={s.span}><button onClick={props.logout}>Logout</button></span>
                        </div>
                    : <span className={s.span}><NavLink to={'/login'}>Login</NavLink></span>
                }
            </div>
        </header>
    );
}

export default Header;