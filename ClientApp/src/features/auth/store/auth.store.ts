import type { User } from "@/types/user.type";
import { create } from "zustand";

interface AuthStore {
  user: User | null;
  isAuthenticated: boolean;
  setUser: (user: User | null) => void;
}

export const useAuthStore = create<AuthStore>((set, get) => ({
  user: null,
  isAuthenticated: get().user !== null,
  setUser: (user: User | null) => set({ user }),
}));
