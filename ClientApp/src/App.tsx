import { Router } from "@/features/router/app.router";
import { ThemeProvider } from "@/features/theme/providers/theme.provider";

function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="ui-theme">
      <Router />
    </ThemeProvider>
  );
}

export default App;
