import './App.css';

const App = () => {
  return (
    <div>
      <Header />
      <Technologies />
    </div>
  );
}

const Technologies = () => {
  return (
    <div>
        <ul>
          <li>
            Simple HTMP
          </li>
        </ul>
      </div>
  );
}

const Header = () => {
  return (
    <div>
      <a href = '#s'>Home</a>
      <a href = '#s'>Home@</a>
      <a href = '#s'>Home213</a>
    </div>
  );
}

export default App;
