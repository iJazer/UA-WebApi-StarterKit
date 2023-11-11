import { Home } from "./components/Home";
import { Swagger } from "./components/Swagger";

const AppRoutes = [
  {
    index: true,
    element: <Home />
   },
   {
      index: true,
      path: '/swagger',
      element: <Swagger />
   }
];

export default AppRoutes;
